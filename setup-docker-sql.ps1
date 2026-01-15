# ============================================
# AUTO SETUP DOCKER SQL SERVER & SEED DATA
# ============================================

Write-Host "`n=== AUTO SETUP DOCKER SQL SERVER ===" -ForegroundColor Cyan

# Step 1: Check if Docker Desktop is running
Write-Host "`n[1/6] Checking Docker Desktop..." -ForegroundColor Yellow
$dockerRunning = $false
try {
    docker ps | Out-Null
    $dockerRunning = $true
    Write-Host "✓ Docker Desktop is running" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker Desktop is NOT running" -ForegroundColor Red
    Write-Host "`nPlease start Docker Desktop first, then run this script again." -ForegroundColor Yellow
    Write-Host "Opening Docker Desktop..." -ForegroundColor Cyan
    Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    Write-Host "`nWaiting for Docker Desktop to start (30 seconds)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 30
}

# Step 2: Check existing containers
Write-Host "`n[2/6] Checking existing SQL Server containers..." -ForegroundColor Yellow
$existingContainers = docker ps -a --format "{{.Names}}" | Select-String "sqlserver"

if ($existingContainers) {
    Write-Host "Found existing containers:" -ForegroundColor Cyan
    $existingContainers | ForEach-Object { Write-Host "  - $_" -ForegroundColor White }
    
    # Start if stopped
    $existingContainers | ForEach-Object {
        $containerName = $_.ToString()
        $status = docker inspect -f '{{.State.Status}}' $containerName
        if ($status -ne "running") {
            Write-Host "Starting container: $containerName" -ForegroundColor Yellow
            docker start $containerName
            Start-Sleep -Seconds 5
        } else {
            Write-Host "Container already running: $containerName" -ForegroundColor Green
        }
    }
} else {
    Write-Host "No existing SQL Server container found. Creating new one..." -ForegroundColor Yellow
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Zzdigimon@315" `
       -p 1433:1433 --name sqlserver_funews `
       -d mcr.microsoft.com/mssql/server:2022-latest
    
    Write-Host "Waiting for SQL Server to start (15 seconds)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 15
}

# Step 3: Get container name
Write-Host "`n[3/6] Getting container name..." -ForegroundColor Yellow
$containerName = docker ps --format "{{.Names}}" | Select-String "sqlserver" | Select-Object -First 1
if (!$containerName) {
    Write-Host "✗ No running SQL Server container found!" -ForegroundColor Red
    exit 1
}
$containerName = $containerName.ToString()
Write-Host "✓ Using container: $containerName" -ForegroundColor Green

# Step 4: Copy SQL script to container
Write-Host "`n[4/6] Copying SQL script to container..." -ForegroundColor Yellow
docker cp "docker/init-db/FUNewsManagementSystem.sql" "${containerName}:/tmp/setup.sql"
Write-Host "✓ SQL script copied" -ForegroundColor Green

# Step 5: Execute SQL script
Write-Host "`n[5/6] Executing SQL script..." -ForegroundColor Yellow
Write-Host "This may take a few moments..." -ForegroundColor Cyan

docker exec -it $containerName /opt/mssql-tools/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" `
   -i /tmp/setup.sql

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ SQL script executed successfully" -ForegroundColor Green
} else {
    Write-Host "✗ SQL script execution failed" -ForegroundColor Red
    Write-Host "`nTrying alternative method..." -ForegroundColor Yellow
    
    # Alternative: Run script line by line
    $sqlContent = Get-Content "docker/init-db/FUNewsManagementSystem.sql" -Raw
    $sqlContent | docker exec -i $containerName /opt/mssql-tools/bin/sqlcmd `
       -S localhost -U sa -P "Zzdigimon@315"
}

# Step 6: Verify data
Write-Host "`n[6/6] Verifying database and data..." -ForegroundColor Yellow

$verifyQuery = @"
USE FUNewsManagementSystem;
SELECT 'Categories' AS TableName, COUNT(*) AS RecordCount FROM Category
UNION ALL SELECT 'Tags', COUNT(*) FROM Tag
UNION ALL SELECT 'SystemAccounts', COUNT(*) FROM SystemAccount
UNION ALL SELECT 'NewsArticles', COUNT(*) FROM NewsArticle
UNION ALL SELECT 'NewsTag', COUNT(*) FROM NewsTag;
"@

docker exec -it $containerName /opt/mssql-tools/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" `
   -Q "$verifyQuery"

Write-Host "`n=== SETUP COMPLETE ===" -ForegroundColor Green
Write-Host "`nContainer Info:" -ForegroundColor Cyan
Write-Host "  Name: $containerName" -ForegroundColor White
Write-Host "  Port: 1433" -ForegroundColor White
Write-Host "  Database: FUNewsManagementSystem" -ForegroundColor White
Write-Host "  User: sa" -ForegroundColor White
Write-Host "  Password: Zzdigimon@315" -ForegroundColor White

Write-Host "`nConnection String:" -ForegroundColor Cyan
Write-Host "  Server=localhost,1433;Database=FUNewsManagementSystem;User Id=sa;Password=Zzdigimon@315;TrustServerCertificate=True;Encrypt=True;" -ForegroundColor White

Write-Host "`nNext Steps:" -ForegroundColor Cyan
Write-Host "  1. Run API: dotnet run --project 'HuynhHuuToan_ SE1856_A01_BE'" -ForegroundColor Yellow
Write-Host "  2. Open Swagger: https://localhost:7xxx/swagger" -ForegroundColor Yellow
Write-Host "  3. Test endpoints: GET /api/Health, GET /api/Category, GET /api/NewsArticle" -ForegroundColor Yellow

Write-Host "`nPress any key to start the API..." -ForegroundColor Green
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Start API
Write-Host "`nStarting API..." -ForegroundColor Cyan
Set-Location "HuynhHuuToan_ SE1856_A01_BE"
dotnet run
