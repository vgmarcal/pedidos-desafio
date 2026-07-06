# Etapa 1: compila o Angular (gera os arquivos estáticos em dist/)
FROM node:24-alpine AS build
WORKDIR /app

# Copia só os manifests primeiro para aproveitar o cache de camadas do Docker:
# o npm ci só roda de novo se o package.json/package-lock.json mudarem.
COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build

# Etapa 2: serve os estáticos com nginx (imagem final não tem Node)
FROM nginx:1.29-alpine
COPY docker/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/pedidos-web/browser /usr/share/nginx/html
EXPOSE 80
