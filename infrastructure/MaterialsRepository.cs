using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure;

public class MaterialsRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public MaterialsRepository (NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public IEnumerable<Material> GetAllMaterials()
    {
        const string sql = @$"SELECT id as {nameof(Material.Id)}, material_name as {nameof(Material.Name)} FROM box_factory.materials";
        
        using var conn = _dataSource.OpenConnection();
        return conn.Query<Material>(sql);
    }

    public Material GetMaterialsById(int id)
    {
        const string sql =
            $@"SELECT id as {nameof(Material.Id)}, material_name as {nameof(Material.Name)} FROM box_factory.materials WHERE id = @id";
        using var conn = _dataSource.OpenConnection();
        return conn.QueryFirst<Material>(sql, id);
    }

    public Material CreateMaterial(Material material)
    {
        const string sql =
            $@"INSERT INTO box_factory.materials (material_name) VALUES (@{nameof(Material.Name)}) RETURNING id as {nameof(Material.Id)}, material_name as {nameof(Material.Name)}";
        using var conn = _dataSource.OpenConnection();
        return conn.QueryFirst<Material>(sql, material);
    }

    public Material UpdateMaterial(Material material)
    {
        const string sql =
            $@"UPDATE box_factory.materials SET material_name = {nameof(Material.Name)} WHERE id = @{nameof(Material.Id)} RETURNING id as {nameof(Material.Id)}, material_name as {nameof(Material.Name)}";
        using var conn = _dataSource.OpenConnection();
        return conn.QueryFirst<Material>(sql, material);
    }

    public int DeleteMaterial(int id)
    {
        const string sql =
            $@"DELETE FROM box_factory.materials WHERE id = @id";
        using var conn = _dataSource.OpenConnection();
        return conn.Execute(sql, id);
    }
}