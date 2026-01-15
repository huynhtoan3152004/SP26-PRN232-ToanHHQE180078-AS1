# Quick Start - Docker Compose (SQL Server + API)
Write-Host "=== Starting FU News Management System ===" -ForegroundColor Cyan
Write-Host "Docker Compose will start SQL Server and API containers" -ForegroundColor Gray

# Stop any running API outside Docker
Write-Host "`nStopping any API running outside Docker..." -ForegroundColor Gray
Get-Process | Where-Object {$_.ProcessName -like "*HuynhHuuToan*"} | Stop-Process -Force -ErrorAction SilentlyContinue

# Get script directory and navigate to docker folder
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location (Join-Path $scriptPath "docker")

# 1. Check if containers are already running
Write-Host "`n[1/5] Checking existing containers..." -ForegroundColor Yellow
$existingContainers = docker ps -a --filter "name=sql2022" --filter "name=funews-api" --format "{{.Names}}"
if ($existingContainers) {
    Write-Host "Found existing containers. Stopping and removing..." -ForegroundColor Gray
    docker-compose down
}
Write-Host "✓ Ready to start fresh" -ForegroundColor Green

# 2. Build and start containers
Write-Host "`n[2/5] Building and starting containers..." -ForegroundColor Yellow
Write-Host "This may take a few minutes on first run..." -ForegroundColor Gray
docker-compose up -d --build
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to start containers" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Containers started" -ForegroundColor Green

# 3. Wait for SQL Server to be healthy
Write-Host "`n[3/5] Waiting for SQL Server to be ready..." -ForegroundColor Yellow
$maxAttempts = 30
$attempt = 0
while ($attempt -lt $maxAttempts) {
    $health = docker inspect --format='{{.State.Health.Status}}' sql2022 2>$null
    if ($health -eq "healthy") {
        Write-Host "✓ SQL Server is healthy" -ForegroundColor Green
        break
    }
    Write-Host "Waiting... ($attempt/$maxAttempts)" -ForegroundColor Gray
    Start-Sleep -Seconds 2
    $attempt++
}
if ($attempt -eq $maxAttempts) {
    Write-Host "❌ SQL Server failed to become healthy" -ForegroundColor Red
    docker-compose logs sqlserver
    exit 1
}

# 4. Seed database
Write-Host "`n[4/5] Seeding database..." -ForegroundColor Yellow
docker cp "init-db/FUNewsManagementSystem.sql" sql2022:/tmp/setup.sql
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -i /tmp/setup.sql
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Database seeded successfully" -ForegroundColor Green
} else {
    Write-Host "⚠ Warning: Database seeding may have failed" -ForegroundColor Yellow
}

# 5. Verify and show info
Write-Host "`n[5/5] Verifying system..." -ForegroundColor Yellow
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -Q "USE FUNewsManagementSystem; SELECT 'Categories' AS TableName, COUNT(*) AS RecordCount FROM Category UNION ALL SELECT 'Tags', COUNT(*) FROM Tag UNION ALL SELECT 'SystemAccounts', COUNT(*) FROM SystemAccount UNION ALL SELECT 'NewsArticles', COUNT(*) FROM NewsArticle;" 2>$null

Write-Host "`n" + "="*60 -ForegroundColor Cyan
Write-Host "✓ FU News Management System is running!" -ForegroundColor Green
Write-Host "="*60 -ForegroundColor Cyan
Write-Host "`nServices:" -ForegroundColor White
Write-Host "  🗄️  SQL Server:  " -NoNewline -ForegroundColor White
Write-Host "localhost:1433" -ForegroundColor Yellow
Write-Host "  🚀 API:          " -NoNewline -ForegroundColor White
Write-Host "http://localhost:5024" -ForegroundColor Yellow
Write-Host "  📚 Swagger UI:   " -NoNewline -ForegroundColor White
Write-Host "http://localhost:5024/swagger" -ForegroundColor Yellow

Write-Host "`nUseful Commands:" -ForegroundColor White
Write-Host "  View API logs:    " -NoNewline -ForegroundColor Gray
Write-Host "docker logs funews-api -f" -ForegroundColor Cyan
Write-Host "  View SQL logs:    " -NoNewline -ForegroundColor Gray
Write-Host "docker logs sql2022 -f" -ForegroundColor Cyan
Write-Host "  Stop all:         " -NoNewline -ForegroundColor Gray
Write-Host "docker-compose down" -ForegroundColor Cyan
Write-Host "  Restart all:      " -NoNewline -ForegroundColor Gray
Write-Host "docker-compose restart" -ForegroundColor Cyan

Write-Host "`nOpening Swagger UI in browser..." -ForegroundColor Gray
Start-Sleep -Seconds 2
Start-Process "http://localhost:5024/swagger"

# Return to root directory
Set-Location ".."
