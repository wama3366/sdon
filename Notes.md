## Application Services:
Service interfaces are only there to allow UI/API testing independently of the implementation.

## Core Domain:
No interfaces. Application services and all other components should depend directly on aggregates, values and entities.
The only useful interfaces are for polymorphism, for example to implement a strategy pattern.

## Set up postgres user
GRANT CONNECT ON DATABASE school_donations TO school_donations_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO school_donations_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO school_donations_user;

GRANT CONNECT ON DATABASE school_donations_test TO school_donations_test_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO school_donations_test_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO school_donations_test_user;

GRANT CONNECT ON DATABASE identity_server TO identity_server_user;
GRANT USAGE, CREATE ON SCHEMA public TO identity_server_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO identity_server_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO identity_server_user;

GRANT CONNECT ON DATABASE identity_server_test TO identity_server_test_user;
GRANT USAGE, CREATE ON SCHEMA public TO identity_server_test_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO identity_server_test_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO identity_server_test_user;

GRANT CONNECT ON DATABASE aspnet_identity TO aspnet_identity_user;
GRANT USAGE, CREATE ON SCHEMA public TO aspnet_identity_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO aspnet_identity_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO aspnet_identity_user;

GRANT CONNECT ON DATABASE aspnet_identity_test TO aspnet_identity_test_user;
GRANT USAGE, CREATE ON SCHEMA public TO aspnet_identity_test_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO aspnet_identity_test_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO aspnet_identity_test_user;

## Identity Server Database Migrations
dotnet ef migrations add Grants -c PersistedGrantDbContext -o Migrations/PersistedGrantDb
dotnet ef migrations add Configuration -c ConfigurationDbContext -o Migrations/ConfigurationDb

dotnet ef migrations script -c PersistedGrantDbContext -o Migrations/PersistedGrantDb.sql
dotnet ef migrations script -c ConfigurationDbContext -o Migrations/ConfigurationDb.sql

## Backup script password set up
1. Correct Filename and Location:
On Windows, the file must be named pgpass.conf (not .pgpass).
It must be located in C:\Users\Wassim\AppData\Roaming\PostgreSQL\
If the PostgreSQL folder doesn't exist there, create it.
2. Open pgpass.conf with a plain text editor (like Notepad).
Add a single line (or multiple lines if you have different hosts/users) in this format:
hostname:port:database:username:password
or
hostname:port:*:username:password
3. Strict File Permissions:
Go to the Security tab.
Click the Advanced button.
Click Disable inheritance (if it's enabled). When prompted, select "Remove all inherited permissions from this object".
Click Add.
Click "Select a principal".
Type the name of your user account (the one you log in as, or the one that will run the scheduled task) and click Check 
4. Names, then OK.
With your user account selected, check the "Full control" checkbox under "Basic permissions".
4. Restart PowerShell/Command Prompt:
After making changes to pgpass.conf or its permissions, always close any open PowerShell/Command Prompt windows and 
5. open new ones for the changes to take effect.

## DevOps docker image build
Building 192.168.0.101:6000/my-dotnet-sdk-with-node:9-22 and then pushing to Unraid docker registry
docker build -t 192.168.0.101:6000/my-dotnet-sdk-with-node:9-22 .
docker push 192.168.0.101:6000/my-dotnet-sdk-with-node:9-22