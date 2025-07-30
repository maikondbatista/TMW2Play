# Use Node.js 20 for building Angular 19
FROM node:20-alpine AS build

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Build the Angular app using docker configuration
RUN npm run build:docker

# Use Nginx to serve the Angular app
FROM nginx:alpine

# Copy the build output to Nginx html directory
COPY --from=build /app/dist/browser/. /usr/share/nginx/html/

# Copy custom Nginx configuration (optional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
