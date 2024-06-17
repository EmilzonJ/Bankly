# Use an official Node.js runtime as the base image
FROM node:20-alpine

# Set the working directory in the container
WORKDIR /app

# Build arguments
ARG VITE_API_BASE_URL

# Set environment variables
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

# Copy the package.json and package-lock.yaml files to the container
COPY package.json ./
COPY pnpm-lock.yaml ./

# Install project dependencies
RUN npm install -g pnpm
RUN pnpm install

# Copy the rest of the project files to the container
COPY . .

# Build the React app
RUN pnpm build

# Expose the port that the server will listen on
EXPOSE 3000

# Start the application
CMD [ "pnpm", "preview" ]