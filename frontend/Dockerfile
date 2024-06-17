FROM node:20.11.1-alpine3.19 AS build

WORKDIR /app

COPY package.json ./

RUN npm install -g pnpm

ENV PATH /app/node_modules/.bin:$PATH

COPY . .

RUN pnpm install
RUN pnpm build

FROM nginx:1.25.4-alpine3.18

COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /var/www/html/

EXPOSE 3000

ENTRYPOINT ["nginx","-g","daemon off;"]