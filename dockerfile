# Building 192.168.0.101:6000/my-dotnet-sdk-with-node:9-22 and then pushing to Unraid docker registry
# docker build -t 192.168.0.101:6000/my-dotnet-sdk-with-node:9-22 .
# docker push 192.168.0.101:6000/my-dotnet-sdk-with-node:9-22

FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install curl and gnupg for adding NodeSource repo
RUN apt-get update && apt-get install -y curl gnupg

# Add Node.js 18.x repository and install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && apt-get install -y nodejs

# Clean up
RUN apt-get clean && rm -rf /var/lib/apt/lists/*

# Verify
RUN dotnet --version
RUN node --version
RUN npm --version