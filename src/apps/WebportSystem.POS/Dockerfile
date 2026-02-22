FROM node:20

# Install Angular CLI
RUN npm install -g @angular/cli

# App directory
WORKDIR /app

# Install deps first (better caching)
# Copy package.json from the Angular app directory
COPY src/apps/angular-pos/package*.json ./
RUN npm install

# Copy the Angular app source
COPY src/apps/angular-pos/ .

# Expose Angular dev server
EXPOSE 4200

# Run Angular
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "4200"]



