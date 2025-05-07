# Gerenciador de Produtos

Aplicação web completa para gerenciamento de produtos com interface intuitiva e responsiva, construída com arquitetura moderna e escalável.

## Funcionalidades

- Cadastro de produtos com código, nome, descrição e preço
- Edição e exclusão de produtos
- Pesquisa em tempo real
- Validação de formulários
- Interface responsiva com Tailwind CSS
- Formatação automática de valores monetários (R$)
- Notificações de sucesso/erro
- CRUD completo via API REST

## Arquitetura

O projeto segue uma arquitetura limpa (Clean Architecture) com as seguintes camadas:

- **API**: Backend em .NET Core 8
  - Controllers RESTful
  - Validação de dados
  - Tratamento de erros
  - Documentação Swagger

- **Application**: Lógica de negócios
  - Handlers para comandos e consultas
  - Validações de negócio
  - DTOs e mapeamentos

- **Domain**: Modelos de domínio
  - Entidades
  - Interfaces de repositório
  - Exceções de domínio

- **Infrastructure**: Implementações de infraestrutura
  - Entity Framework Core
  - Repositórios
  - Configurações do banco de dados

- **Web**: Interface do usuário
  - HTML5 semântico
  - Tailwind CSS para estilização
  - JavaScript/jQuery para interatividade
  - Responsividade para todos os dispositivos

## Tecnologias

- **Backend**:
  - .NET 8
  - Entity Framework Core
  - SQL Server
  - Docker

- **Frontend**:
  - HTML5
  - Tailwind CSS
  - JavaScript (ES6+)
  - jQuery
  - SweetAlert2 para notificações


### Banco de Dados

- **Servidor**: SQL Server 2019 (Docker)
- **Porta**: 1433
- **Usuário**: sa
- **Senha**: asdf1234ASDF!
- **Banco de Dados**: ProductManagerDb

## Executando a Aplicação

### Usando Docker (Recomendado)

1. **Iniciar todos os serviços**
   ```bash
   docker-compose up -d --build
   ```

2. **Acessar a aplicação**
   - Interface Web: [http://localhost:80](http://localhost:80)
   - API: [http://localhost:5006](http://localhost:5006)
   - Banco de Dados: localhost,1433

### Desenvolvimento Local

1. **Iniciar o SQL Server**
   ```bash
   docker-compose up -d sqlserver init-db
   ```

2. **Executar a API**
   ```bash
   cd src/API/ProductManager.API
   dotnet run
   ```

3. **Executar o frontend**
   ```bash
   cd web
   npx http-server -p 3000
   ```

4. **Acessar**
   - Frontend: [http://localhost:3000](http://localhost:3000)
   - API: [http://localhost:5006](http://localhost:5006)

## Documentação da API

### Produtos

#### Listar todos os produtos
```
GET /api/Products
```

#### Buscar produto por ID
```
GET /api/Products/{id}
```

#### Pesquisar produtos
```
GET /api/Products/search?term={termo}
```

#### Criar produto
```
POST /api/Products
```
**Request Body:**
```json
{
  "code": "P001",
  "name": "Produto Teste",
  "description": "Descrição do produto",
  "price": 100.50,
  "isActive": true
}
```

#### Atualizar produto
```
PUT /api/Products/{id}
```
**Request Body:**
```json
{
  "id": 1,
  "code": "P001",
  "name": "Produto Atualizado",
  "description": "Nova descrição",
  "price": 150.00,
  "isActive": true
}
```

### Excluir produto (exclusão lógica)
```
DELETE /api/Products/{id}
```

## Estrutura do Projeto

- **src/API/ProductManager.API**: Camada de API
- **src/Application/ProductManager.Application**: Regras de negócio e casos de uso
- **src/Domain/ProductManager.Domain**: Entidades e interfaces de domínio
- **src/Infrastructure/ProductManager.Infrastructure**: Implementação de persistência e serviços externos

## Migrações do Banco de Dados

As migrações são aplicadas automaticamente ao iniciar a aplicação.
