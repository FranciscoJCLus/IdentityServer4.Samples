FROM microsoft/aspnet:1.0.0-rc1-update1-coreclr

COPY project.json /app/

WORKDIR /app

RUN ["dnu", "restore"]

ADD . /app

RUN apt-get update -y && apt-get install --no-install-recommends -y -q \
    curl \
    python \
    build-essential \
    git \
    ca-certificates

RUN mkdir /nodejs && \
    curl http://nodejs.org/dist/v0.10.33/node-v0.10.33-linux-x64.tar.gz | \
    tar xvzf - -C /nodejs --strip-components=1

ENV PATH $PATH:/nodejs/bin

RUN npm install -g gulp-cli bower

RUN ["npm", "install", "."]
RUN ["bower", "install"]
RUN ["gulp", "clean"]
RUN ["gulp", "min"]

EXPOSE 22530

ENTRYPOINT ["dnx","web"]
