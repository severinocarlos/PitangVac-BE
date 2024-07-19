# PitangVac

Repositório destinado ao desafio final do programa de estágio da Pitang:

### Como rodar:

> Para rodar o projeto você precisa ter instalado na sua máquina os seguintes componentes:

## Instalação do .NET 6

1. Acesse o site oficial do .NET: [Download .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Selecione o instalador apropriado para o seu sistema operacional (Windows, macOS, Linux).
3. Siga as instruções de instalação fornecidas pelo instalador.

### Verificação da Instalação

Após a instalação, abra um terminal e execute o comando abaixo para verificar se o .NET 6 foi instalado corretamente:

```sh
dotnet --version
```


## Instação do Visual Studido

1. Acesse o site e baixar para rodar o projeto: [Visual Studio](https://visualstudio.microsoft.com/pt-br/vs/community/)

## Instalação do SQL Server

1. Acesse e baixe o banco que iremos usar: [SQL Server](https://www.microsoft.com/pt-br/download/details.aspx?id=101064)

## Instalação do gerenciador do SQL Server

1. Baixe usando esse link: [SQL Server Management](https://miro.com/app/board/uXjVKQKwmYc=/?moveToWidget=3458764586901568319&cot=14)


> Logo após você precisar levantar o servidor do banco para que ele seja acessível usando o SQL Management. Com SQL management aberto você irá se conectar e rodar o `script.sql` que está na pasta raiz do projeto. 

> Com o banco criado você irá no seu Visual Studio e abrir a solução para levantar a API.

**Caso tenha problemas com a conexão com o banco verifique o arquivo `appsettings.json` que possui a string de conexão com o banco**