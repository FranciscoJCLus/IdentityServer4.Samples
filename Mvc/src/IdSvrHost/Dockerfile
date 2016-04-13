FROM microsoft/aspnet:1.0.0-rc1-update1-coreclr

COPY . /IdSvrHost
WORKDIR /IdSvrHost

RUN ["dnu", "restore"]

EXPOSE 22530

CMD ["web"]
ENTRYPOINT ["dnx"]
