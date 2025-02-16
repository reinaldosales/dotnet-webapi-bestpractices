# dotnet-webapi-bestpractices
Esse projeto conta as melhores práticas para construir uma web api.

### 1. Uso de versionamento
O versionamento permite manter compatibilidade com versões antigas da API enquanto novas funcionalidades são adicionadas. O versionamento pode ser implementado via URL (ex: /api/v1/resource), headers ou query parameters.
### 2. Autenticação e autorização com JWT
JSON Web Token (JWT) é usado para autenticação segura e sem estado. A API valida o token recebido no header de cada requisição, garantindo que apenas usuários autorizados acessem determinados endpoints.
### 3. Programação assíncrona
O uso de async e await melhora o desempenho da API, evitando bloqueios desnecessários e permitindo que múltiplas requisições sejam tratadas simultaneamente.
### 4. Convenções RESTful
A API segue padrões RESTful, utilizando verbos HTTP corretamente (GET, POST, PUT, DELETE), retornando códigos de status apropriados (200, 201, 400, 404) e organizando endpoints de forma intuitiva.
### 5. Validação na entrada de dados (Uso de fluent validation)
O FluentValidation é utilizado para validar requisições de entrada, garantindo que os dados enviados pelo cliente atendam aos requisitos definidos antes de serem processados.
### 6. Práticas de segurança com EF Core.
Inclui uso de Migrations, Parameterization Queries para evitar SQL Injection e aplicação de restrições no banco para maior segurança e integridade dos dados.
### 7. Monitoramento e métricas (Uso de elasticapm)
O Elastic APM é integrado para monitorar a performance da API, capturar logs de erros e coletar métricas, ajudando na identificação de gargalos e melhorias.
### 8. Uso de API Documentation (Swagger, Scalar)
O Swagger e o Scalar são usados para documentar os endpoints da API, permitindo testes interativos e facilitando a integração com outros sistemas.
### 9. Praticas de otimização para banco de dados
Uso de índices, queries otimizadas, lazy loading quando necessário, além de técnicas como batching e caching de consultas para melhorar a performance do banco.
### 10. Filtragem de dados e paginação
A API implementa filtros dinâmicos para consultas eficientes e paginação para evitar retornos de grandes volumes de dados, garantindo melhor performance e usabilidade.
### 11. Uso de cache (Uso de hybride cache)
O Hybrid Cache é utilizado para armazenar dados frequentemente acessados, reduzindo chamadas ao banco e melhorando o tempo de resposta da API.
### 12. Uso de minimal api
A implementação de Minimal APIs reduz a complexidade do código, melhorando a performance e simplificando a criação de endpoints, sem necessidade de Controllers tradicionais.
