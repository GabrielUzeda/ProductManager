# Product Manager API

API para gerenciamento de produtos com operações CRUD e pesquisa.

## Requisitos

- Docker e Docker Compose
- .NET 8.0 SDK

## Configuração

1. **Banco de Dados**
   - A aplicação utiliza SQL Server rodando em um contêiner Docker
   - O script de inicialização do banco de dados está em `bd/sqlserver/init-scripts/init.sql`

2. **Variáveis de Ambiente**
   - As configurações de conexão com o banco de dados estão em `src/API/ProductManager.API/appsettings.json`
   - Credenciais padrão do SQL Server:
     - Usuário: sa
     - Senha: asdf1234ASDF!
     - Porta: 1433
     - Nome do banco: ProductManagerDb

## Executando a Aplicação

1. **Iniciar o SQL Server**
   ```bash
   docker-compose up -d
   ```

2. **Executar a aplicação**
   ```bash
   cd src/API/ProductManager.API
   dotnet run
   ```

   A aplicação estará disponível em: `http://localhost:5006`

## Endpoints da API

### Listar todos os produtos ativos
```
GET /api/Products
```

### Buscar produto por ID
```
GET /api/Products/{id}
```

### Pesquisar produtos
```
GET /api/Products/search?term={termo}
```

### Criar novo produto
```
POST /api/Products
```
Exemplo de corpo da requisição:
```json
{
  "code": "P001",
  "name": "Produto Teste",
  "description": "Descrição do produto",
  "price": 100.00
}
```

### Atualizar produto
```
PUT /api/Products/{id}
```
Exemplo de corpo da requisição:
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
