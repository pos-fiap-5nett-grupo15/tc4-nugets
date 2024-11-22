# Como publicar uma nova versão:

## Acesso ao NuGet.org:
- PASSO 1: Realize o login no NuGet.org (https://www.nuget.org/users/account/LogOn)
    - Utilize o login via Microsoft:
        - nugettechchallenge@gmail.com
        - nuget.tech.challenge2025
        OBS: Caso um código de confirmação seja enviado ao gmail, utilize os acessos abaixo para entrar no Gmail e obter o código:
            - nugettechchallenge@gmail.com
            - nugettechchallenge2025
- PASSO 2: Após entrar no NuGet.org, cliquei no perfil no canto superior direito:
    - Acesse a opção "Manage Packages" e depois clique em "Published Packages", você verá todos os pacotes publicados atualmente.

## Para publicar um novo pacote:
- PASSO 1: Realize as alterações desejadas no código.
- PASSO 2: Altere o número da versão contido na tag <Version> em cada arquivo `.csproj` que recebeu alteração no código.
- PASSO 2: Compile o Projeto:
    - Via Visual Studio: **CTRL + SHIFT + B**
    - ou Via terminal, dentro da pasta do projeto, execute: `dotnet build .\TechChallenge3.Nugets.sln`
- PASSO 3: O comando anterior vai gerar um novo Pacote NuGet, isso criará um arquivo `.nupkg` na pasta `bin/Debug` de cada `.csproj`
- PASSO 4: Acesse o site do NuGet.org
- PASSO 5: No centro superior, acesse "Upload"
- PASSO 6: Clique em "Browse..." e selecione cada arquivo `.nupkg` desejado (o upload é feito um arquivo por vez)
- PASSO 7: Clique em "Submit" e após esse processo, o pacote será processado (cerca de 10/15 minutos) e disponibilizado no NuGet.org.
- PASSo 8: Por fim, retorne ao projeto que possui dependência e atualize o NuGet para a nova versão.