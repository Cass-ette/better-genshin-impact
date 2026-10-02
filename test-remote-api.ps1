# BetterGI Remote Control API Test Script
# Usage: .\test-remote-api.ps1 -Token "your-api-token" -Port 8080

param(
    [Parameter(Mandatory=$true)]
    [string]$Token,

    [Parameter(Mandatory=$false)]
    [int]$Port = 8080
)

$headers = @{
    "X-API-Token" = $Token
}

$baseUrl = "http://127.0.0.1:$Port"

Write-Host "Testing BetterGI Remote Control API at $baseUrl" -ForegroundColor Cyan
Write-Host ""

# Test 1: Get Status
Write-Host "1. Testing GET /api/status..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/status" -Method Get -Headers $headers
    Write-Host "   Success: $($response | ConvertTo-Json -Compress)" -ForegroundColor Green
} catch {
    Write-Host "   Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 2: Start Task
Write-Host "2. Testing POST /api/start..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/start" -Method Post -Headers $headers
    Write-Host "   Success: $($response | ConvertTo-Json -Compress)" -ForegroundColor Green
} catch {
    Write-Host "   Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Start-Sleep -Seconds 2

# Test 3: Get Status Again
Write-Host "3. Testing GET /api/status (after start)..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/status" -Method Get -Headers $headers
    Write-Host "   Success: $($response | ConvertTo-Json -Compress)" -ForegroundColor Green
} catch {
    Write-Host "   Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 4: Stop Task
Write-Host "4. Testing POST /api/stop..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/stop" -Method Post -Headers $headers
    Write-Host "   Success: $($response | ConvertTo-Json -Compress)" -ForegroundColor Green
} catch {
    Write-Host "   Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "API Test Complete!" -ForegroundColor Cyan
