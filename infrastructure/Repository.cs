using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure;

public class Repository
{
    private readonly NpgsqlDataSource _dataSource;

    public Repository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Box> GetAllBoxes()
    {
        const string sql = @$"SELECT 
        guid as {nameof(Box.Guid)},
        width as {nameof(Box.Width)},
        height as {nameof(Box.Height)},
        depth as {nameof(Box.Depth)},
        location as {nameof(Box.Location)},
        description as {nameof(Box.Description)}

        FROM box_factory.box_inventory";
        
        using var con = _dataSource.OpenConnection();
        return con.Query<Box>(sql);
    }

    public Box GetBoxByGuid(Guid guid)
    {
        const string sql = @$"SELECT 
        guid as {nameof(Box.Guid)},
        width as {nameof(Box.Width)},
        height as {nameof(Box.Height)},
        depth as {nameof(Box.Depth)},
        location as {nameof(Box.Location)},
        description as {nameof(Box.Description)}

        FROM box_factory.box_inventory
        where guid = @guid";
        
        using var con = _dataSource.OpenConnection();
        return con.QuerySingle<Box>(sql, new {guid});
    }

    public Box CreateBox(Box box)
    {
        const string sql = @$"INSERT INTO box_factory.box_inventory (width, height, depth, location, description) 
        VALUES (@{nameof(Box.Width)}, @{nameof(Box.Height)}, @{nameof(Box.Depth)}, @{nameof(Box.Location)}, @{nameof(Box.Description)})
        RETURNING 
            guid as {nameof(Box.Guid)}, 
            width as {nameof(Box.Width)}, 
            height as {nameof(Box.Height)}, 
            depth as {nameof(Box.Depth)}, 
            location as {nameof(Box.Location)}, 
            description as {nameof(Box.Description)}";
        
        using var con = _dataSource.OpenConnection();
        return con.QuerySingle<Box>(sql, box);
    }
}