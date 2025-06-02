# 📊 Sharp Guard

API desenvolvida em **.NET** para gerenciamento de usuários, com autenticação JWT, envio de e-mail e cobertura de testes.

---

## 📦 Rodando o Projeto Localmente

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/) (opcional)
- [MySQL Server (caso não use Docker)](https://dev.mysql.com/downloads/installer/)

---

### 📥 Clone o projeto

```bash
git clone https://github.com/seu-usuario/ManagementAPI.git
cd ManagementAPI
```

---

### ⚙️ Configuração do Ambiente

1. Copie o `.env`:

```env
DB_CONNECTION_STRING=server=db;database=management_cs_api;user=root;password=123456;
JWT_ISSUER=http://localhost:5215/
JWT_AUDIENCE=http://localhost:5215/
JWT_SECRET=enfonefoJONQOFNeoDJKfEIONFIOCeju
JWT_EXPIRE=30
EMAIL_SENDER=teste@gmail.com
EMAIL_SENDER_APP_PASSWORD=app-pass-key
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587
```

2. Atualize as variáveis no `.env` com seus valores reais.

---

### 🐳 Subindo o container Docker do projeto junto com MySQL 

Dentro da pasta do projeto:

```bash
docker compose up -d
```

> 📌 *Caso prefira usar um MySQL local, ajuste a string de conexão no arquivo `.env`.*


---

## 🧪 Executando os testes localmente

Navegue até a pasta de testes:

```bash
cd ManagementAPI.Tests
dotnet test
```

(também é possível testar a través da UI do Visual Studio)
---

## 📊 Gerando Relatório de Cobertura de Testes

1. Instale o ReportGenerator (caso ainda não tenha):

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

2. Execute os testes com cobertura:

```bash
dotnet test ManagementAPI.Tests.csproj --collect:"XPlat Code Coverage"
```

3. Gere o relatório HTML:

```bash
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html
```

---

## 🎯 Objetivo do Projeto

Refatorar um código-fonte legado, aplicando princípios de **Clean Code** e boas práticas de engenharia de software, sem alterar sua funcionalidade.

---

## 📌 Atividades Realizadas

- Refatoração com foco em **legibilidade** e **manutenção**
- Aplicação de pelo menos um **Design Pattern**
- Implementação de **testes unitários**
- Controle de versão público via **GitHub**

---

## ✅ Critérios Atendidos

- 📚 Nomes de variáveis e métodos claros
- 📦 Estrutura modular e coesa
- 📑 Comentários apenas quando necessário
- 🧭 Aplicação de princípios **SOLID**, **DRY**, **KISS** e **YAGNI**
- 🔍 Cobertura de testes com relatório visual
- 📌 Histórico de commits organizado entre abril e maio

---

## 📅 Entregas

- 📄 PDF enviado via **Ulife**, contendo:
  - Código original e deficiências
  - Código refatorado com justificativas
  - Testes unitários implementados
  - Conclusão sobre a importância de **Clean Code**
- 📦 Repositório público no **GitHub** com código e testes

[📎 Link para o arquivo de entrega](https://docs.google.com/document/d/12hYDcAg29dHkn7aEFC8bcyYv-Xd_OSCnnwZ3GkMcCf8/edit?usp=sharing)

---


## 📑 Licença

Este projeto é acadêmico e sem fins comerciais.