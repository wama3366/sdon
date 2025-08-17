# --- Configuration (EDIT THESE VALUES) ---
$PgHost = "localhost"
$PgPort = "5432"
$PgUser = "postgres" # User with backup privileges
$BackupDir = "X:\Projects\DB backups" # Where backups will be saved
$PgBinPath = "C:\Program Files\PostgreSQL\17\bin" # <--- EDIT THIS: Your PostgreSQL 'bin' directory

# --- End Configuration ---

# Construct full paths to PostgreSQL executables
$PgDumpAll = Join-Path $PgBinPath "pg_dumpall.exe"
$PgDump = Join-Path $PgBinPath "pg_dump.exe"
$Psql = Join-Path $PgBinPath "psql.exe"

$Timestamp = (Get-Date -Format "yyyyMMdd_HHmmss")

Write-Host "Starting PostgreSQL backup..."

# --- Basic checks for executables and backup directory ---
# Check if PostgreSQL executables are found at the specified path
$PgTools = @($PgDumpAll, $PgDump, $Psql)
foreach ($Tool in $PgTools) {
    if (-not (Test-Path $Tool -PathType Leaf)) {
        Write-Error "Error: PostgreSQL utility '$Tool' not found at '$PgBinPath'."
        Write-Error "Please ensure '$PgBinPath' is correct and contains the PostgreSQL executables."
        exit 1
    }
}

# Create backup directory if it doesn't exist
if (-not (Test-Path $BackupDir)) {
    New-Item -ItemType Directory -Path $BackupDir -Force | Out-Null
    Write-Host "Created backup directory: $BackupDir"
}

# --- 1. Backup Global Objects (roles, tablespaces) ---
$GlobalBackupFile = "postgres_globals_${Timestamp}.sql"
$GlobalBackupPath = Join-Path $BackupDir $GlobalBackupFile

Write-Host "Backing up global objects to $GlobalBackupPath..."
# Using '&' to execute external commands
& $PgDumpAll -h $PgHost -p $PgPort -U $PgUser -g > "$GlobalBackupPath"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Global objects backup successful."
} else {
    Write-Error "Global objects backup failed! Exit code: $LASTEXITCODE"
}

# --- 2. Backup Individual Databases ---
Write-Host "Getting list of databases..."
$Databases = & $Psql -h $PgHost -p $PgPort -U $PgUser -d "postgres" -t -A -c "SELECT datname FROM pg_database WHERE datistemplate = false AND datname <> 'postgres';"
$Databases = $Databases | ForEach-Object { $_.Trim() } | Where-Object { $_ } # Clean up output

if (-not $Databases) {
    Write-Host "No user databases found to backup."
} else {
    Write-Host "Backing up individual databases..."
    foreach ($DB_NAME in $Databases) {
        $DBBackupFile = "${DB_NAME}_${Timestamp}.dump" # Custom format (uncompressed)
        $DBBackupPath = Join-Path $BackupDir $DBBackupFile

        Write-Host "  - Backing up '$DB_NAME' to $DBBackupPath..."
        # -Fc is for custom format, recommended for pg_restore flexibility
        & $PgDump -h $PgHost -p $PgPort -U $PgUser -Fc "$DB_NAME" > "$DBBackupPath"

        if ($LASTEXITCODE -eq 0) {
            Write-Host "    Backup of '$DB_NAME' successful."
        } else {
            Write-Error "    Backup of '$DB_NAME' failed! Exit code: $LASTEXITCODE"
        }
    }
}

Write-Host "PostgreSQL backup process finished."