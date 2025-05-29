# Trabalho Acadêmico

## Rodando o Projeto Localmente

Para rodar o código localmente é necessário um banco Mysql (no nosso caso optamos por usar um container docker)

Dentro da pasta do projeto rodamos os seguintes comandos

```bash
docker compose up
```

(caso não queira usar o docker basta usar um mysql e alterar a url de conexão do arquivo .env)

Copie o .env example e crie um .env colocando os valores reais das variaveis

abrimos outro terminal e rodamos a api:

```bash
dotnet run
```

## 🎯 Objetivo

Refatorar um código-fonte legado, aplicando princípios de **Clean Code** para melhorar a **legibilidade**, **manutenção** e **eficiência** do código, sem alterar sua funcionalidade.

---

## 📌 Atividades

- Identificar problemas e refatorar utilizando boas práticas de **Clean Code**
- Aplicar pelo menos um **Design Pattern**
- Implementar **testes unitários**
- Utilizar **controle de versão no GitHub** com atualizações registradas nos meses de abril e maio

---

## 📝 Critérios de Avaliação

- **Legibilidade:** Nomes claros e código organizado
- **Estrutura:** Modularização e redução de repetições
- **Documentação:** Comentários apenas quando necessário
- **Boas práticas:** Aplicação de princípios **SOLID**, **DRY**, **KISS** e **YAGNI**
- **Testes unitários** implementados e validados
- **Versionamento público** via **GitHub**

---

## 📅 Entregas

### 📌 1 de junho de 2025

- 📄 PDF no **Ulife** contendo:
  - Código original com deficiências identificadas
  - Código refatorado com justificativas das mudanças
  - Testes unitários implementados
  - Conclusão sobre a importância do **Clean Code**
- 📦 Repositório público no **GitHub** com código e testes (link incluso no PDF)

---

[ARQUIVO PARA ENTREGA](https://docs.google.com/document/d/12hYDcAg29dHkn7aEFC8bcyYv-Xd_OSCnnwZ3GkMcCf8/edit?usp=sharing)

Exemplo de .env:

```
DB_CONNECTION_STRING=server=localhost;database=management_cs_api;user=root;password=2004;
JWT_ISSUER=http://localhost:5215/
JWT_AUDIENCE=http://localhost:5215/
JWT_SECRET=IZAqg5Mg2Jv0o09XJAoO1QbiQUHhFl9wlaWibYvePxVS7VZwazsaR4yBYTgA893K
JWT_EXPIRE=30
EMAIL_SENDER=teste@gmail.com
EMAIL_SENDER_NAME=teste
EMAIL_SENDER_APP_PASSWORD=mySecurePass
```

Script SQL:

```
CREATE TABLE `user` (
   `id` INT AUTO_INCREMENT NOT NULL,
   `username` VARCHAR(255) NOT NULL,
   `password` VARCHAR(255) NOT NULL,
   `email` VARCHAR(255) NOT NULL,
   `role` ENUM("admin", "user") NOT NULL default "user",
   `passwordRecovery` VARCHAR(255) NULL,
   `otpCode` VARCHAR(255) NULL,
   `otpExpiration` DATETIME NULL,

   PRIMARY KEY(`id`)
);
```
