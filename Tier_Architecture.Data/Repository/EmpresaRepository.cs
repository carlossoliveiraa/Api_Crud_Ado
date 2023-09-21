using Microsoft.Data.SqlClient;
using Tier_Architecture.Application.Domain;
using Tier_Architecture.Application.Interfaces;

namespace Tier_Architecture.Data.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {

        private readonly string _connectionString;

        public EmpresaRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task Adicionar(Empresa empresa)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Transaction = transaction;

                    try
                    {
                        // Primeiro, insira a empresa
                        cmd.CommandText = @"INSERT INTO Empresas (Nome, Cnpj, InscricaoEstadual, DataAbertura, Site, Email, Telefone, Ativo)
                                   VALUES (@Nome, @Cnpj, @InscricaoEstadual, @DataAbertura, @Site, @Email, @Telefone, @Ativo);
                                   SELECT SCOPE_IDENTITY();";

                        cmd.Parameters.AddWithValue("@Nome", empresa.Nome);
                        cmd.Parameters.AddWithValue("@Cnpj", empresa.Cnpj);
                        cmd.Parameters.AddWithValue("@InscricaoEstadual", empresa.InscricaoEstadual);
                        cmd.Parameters.AddWithValue("@DataAbertura", empresa.DataAbertura);
                        cmd.Parameters.AddWithValue("@Site", empresa.Site);
                        cmd.Parameters.AddWithValue("@Email", empresa.Email);
                        cmd.Parameters.AddWithValue("@Telefone", empresa.Telefone);
                        cmd.Parameters.AddWithValue("@Ativo", empresa.Ativo);

                        // Execute o comando e obtenha o ID da empresa inserida
                        var empresaId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        // Agora, insira os endereços associados à empresa
                        foreach (var endereco in empresa.Enderecos)
                        {
                            cmd.Parameters.Clear(); // Limpe os parâmetros anteriores
                            cmd.CommandText = @"INSERT INTO Enderecos (EmpresaId, Logradouro, Numero, Bairro, Cidade, UF, Ativo)
                                       VALUES (@EmpresaId, @Logradouro, @Numero, @Bairro, @Cidade, @UF, @Ativo);
                                       SELECT SCOPE_IDENTITY();";

                            cmd.Parameters.AddWithValue("@EmpresaId", empresaId); // Associe o endereço à empresa
                            cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                            cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                            cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                            cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                            cmd.Parameters.AddWithValue("@UF", endereco.UF);
                            cmd.Parameters.AddWithValue("@Ativo", endereco.Ativo);

                            // Execute o comando e obtenha o ID do endereço inserido
                            var enderecoId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                            // Associe o ID do endereço à empresa (pode ser necessário ajustar seu modelo de dados aqui)
                            endereco.Id = enderecoId;
                        }

                        transaction.Commit(); // Confirma a transação
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); // Em caso de exceção, faz um rollback da transação
                        throw; // Propaga a exceção para o chamador
                    }
                }
            }
        }



        public async Task Atualizar(Empresa empresa)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Transaction = transaction;

                    try
                    {
                        // Atualize a empresa
                        cmd.CommandText = @"UPDATE Empresas 
                                   SET Nome = @Nome, Cnpj = @Cnpj, InscricaoEstadual = @InscricaoEstadual, 
                                       DataAbertura = @DataAbertura, Site = @Site, Email = @Email, 
                                       Telefone = @Telefone, Ativo = @Ativo
                                   WHERE Id = @Id";

                        cmd.Parameters.AddWithValue("@Id", empresa.Id);
                        cmd.Parameters.AddWithValue("@Nome", empresa.Nome);
                        cmd.Parameters.AddWithValue("@Cnpj", empresa.Cnpj);
                        cmd.Parameters.AddWithValue("@InscricaoEstadual", empresa.InscricaoEstadual);
                        cmd.Parameters.AddWithValue("@DataAbertura", empresa.DataAbertura);
                        cmd.Parameters.AddWithValue("@Site", empresa.Site);
                        cmd.Parameters.AddWithValue("@Email", empresa.Email);
                        cmd.Parameters.AddWithValue("@Telefone", empresa.Telefone);
                        cmd.Parameters.AddWithValue("@Ativo", empresa.Ativo);

                        await cmd.ExecuteNonQueryAsync();

                        // Atualize os endereços associados à empresa
                        foreach (var endereco in empresa.Enderecos)
                        {
                            cmd.Parameters.Clear(); // Limpe os parâmetros anteriores
                            cmd.CommandText = @"UPDATE Enderecos 
                                       SET Logradouro = @Logradouro, Numero = @Numero, Bairro = @Bairro, 
                                           Cidade = @Cidade, UF = @UF, Ativo = @Ativo
                                       WHERE Id = @Id";

                            cmd.Parameters.AddWithValue("@Id", endereco.Id);
                            cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                            cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                            cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                            cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                            cmd.Parameters.AddWithValue("@UF", endereco.UF);
                            cmd.Parameters.AddWithValue("@Ativo", endereco.Ativo);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit(); // Confirma a transação
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); // Em caso de exceção, faz um rollback da transação
                        throw; // Propaga a exceção para o chamador
                    }
                }
            }
        }
                 
        public async Task<Empresa> ObterPorId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Substitua "Empresas" pelo nome real da tabela de empresas em seu banco de dados
                var query = "SELECT * FROM Empresas WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    // Execute a consulta e obtenha o resultado
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var empresa = new Empresa();
                            empresa.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            empresa.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                            empresa.Cnpj = reader.GetString(reader.GetOrdinal("Cnpj"));
                            empresa.InscricaoEstadual = reader.GetString(reader.GetOrdinal("InscricaoEstadual"));
                            empresa.DataAbertura = reader.GetDateTime(reader.GetOrdinal("DataAbertura"));
                            empresa.Site = reader.GetString(reader.GetOrdinal("Site"));
                            empresa.Email = reader.GetString(reader.GetOrdinal("Email"));
                            empresa.Telefone = reader.GetString(reader.GetOrdinal("Telefone"));
                            empresa.Ativo = reader.GetBoolean(reader.GetOrdinal("Ativo"));

                            // Carregue os endereços associados à empresa
                            empresa.Enderecos = await ObterEnderecosPorEmpresaId(id);

                            return empresa;
                        }
                        return null; // Nenhum registro encontrado com o ID especificado
                    }
                }
            }
        }

        private async Task<List<Endereco>> ObterEnderecosPorEmpresaId(int empresaId)
        {
            var enderecos = new List<Endereco>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Substitua "Enderecos" pelo nome real da tabela de endereços em seu banco de dados
                var query = "SELECT * FROM Enderecos WHERE EmpresaId = @EmpresaId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmpresaId", empresaId);

                    // Execute a consulta e obtenha os resultados
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var endereco = new Endereco();
                            endereco.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            endereco.EmpresaId = reader.GetInt32(reader.GetOrdinal("EmpresaId"));
                            endereco.Logradouro = reader.GetString(reader.GetOrdinal("Logradouro"));
                            endereco.Numero = reader.GetString(reader.GetOrdinal("Numero"));
                            endereco.Bairro = reader.GetString(reader.GetOrdinal("Bairro"));
                            endereco.Cidade = reader.GetString(reader.GetOrdinal("Cidade"));
                            endereco.UF = reader.GetString(reader.GetOrdinal("UF"));
                            endereco.Ativo = reader.GetBoolean(reader.GetOrdinal("Ativo"));

                            enderecos.Add(endereco);
                        }
                    }
                }
            }

            return enderecos;
        }


        public async Task<IEnumerable<Empresa>> ObterTodos()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Substitua "Empresas" pelo nome real da tabela de empresas em seu banco de dados
                var query = "SELECT * FROM Empresas";

                using (var cmd = new SqlCommand(query, connection))
                {
                    // Execute a consulta e obtenha os resultados
                    var results = new List<Empresa>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var empresa = new Empresa();
                            empresa.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            empresa.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                            empresa.Cnpj = reader.GetString(reader.GetOrdinal("Cnpj"));
                            empresa.InscricaoEstadual = reader.GetString(reader.GetOrdinal("InscricaoEstadual"));
                            empresa.DataAbertura = reader.GetDateTime(reader.GetOrdinal("DataAbertura"));
                            empresa.Site = reader.GetString(reader.GetOrdinal("Site"));
                            empresa.Email = reader.GetString(reader.GetOrdinal("Email"));
                            empresa.Telefone = reader.GetString(reader.GetOrdinal("Telefone"));
                            empresa.Ativo = reader.GetBoolean(reader.GetOrdinal("Ativo"));

                            // Carregue os endereços associados à empresa
                            empresa.Enderecos = await ObterEnderecosPorEmpresaId(empresa.Id);

                            results.Add(empresa);
                        }
                    }
                    return results;
                }
            }
        }

        public async Task Remover(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Transaction = transaction;

                    try
                    {
                        // Exclua a empresa
                        cmd.CommandText = "DELETE FROM Empresas WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", id);
                        await cmd.ExecuteNonQueryAsync();

                        // Exclua os endereços associados à empresa
                        cmd.CommandText = "DELETE FROM Enderecos WHERE EmpresaId = @EmpresaId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@EmpresaId", id);
                        await cmd.ExecuteNonQueryAsync();

                        transaction.Commit(); // Confirma a transação
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); // Em caso de exceção, faz um rollback da transação
                        throw; // Propaga a exceção para o chamador
                    }
                }
            }
        }

    }
}
