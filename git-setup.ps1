# Git setup script for Windows PowerShell

Write-Host "=== GIT SETUP FOR SP26-PRN232-ToanHHQE180078-AS1 ===" -ForegroundColor Green

# Initialize git
Write-Host "`n1. Initializing Git..." -ForegroundColor Yellow
git init

# Add all files
Write-Host "`n2. Adding all files..." -ForegroundColor Yellow
git add .

# First commit
Write-Host "`n3. Creating first commit..." -ForegroundColor Yellow
git commit -m "Initial commit: Full CRUD with Generic Repository, Unit of Work, and Query Extensions (Search, Sort, Paging, Selection, Expansion)"

# Rename branch to main
Write-Host "`n4. Renaming branch to main..." -ForegroundColor Yellow
git branch -M main

# Add remote origin
Write-Host "`n5. Adding remote origin..." -ForegroundColor Yellow
git remote add origin https://github.com/huynhtoan3152004/SP26-PRN232-ToanHHQE180078-AS1.git

# Push to GitHub
Write-Host "`n6. Pushing to GitHub..." -ForegroundColor Yellow
Write-Host "Running: git push -u origin main" -ForegroundColor Cyan
git push -u origin main

Write-Host "`n=== DONE! Repository pushed to GitHub ===" -ForegroundColor Green
Write-Host "URL: https://github.com/huynhtoan3152004/SP26-PRN232-ToanHHQE180078-AS1" -ForegroundColor Cyan
