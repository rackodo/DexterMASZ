FROM node:16.17 AS compile-frontend
WORKDIR /frontend

COPY dexter/package.json /opt/dexter/package.json
RUN cd /opt/dexter && npm install -g @angular/cli && npm install
COPY /opt/dexter/ .

COPY dexter/ .

RUN ng build --configuration production --output-hashing=all

FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY static/ /var/www/data/static/
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=compile-frontend /frontend/dist/dexter/ /var/www/data/

# Set timezone
ENV TZ=Europe/Berlin
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

CMD ["nginx", "-g", "daemon off;"]

EXPOSE 80
