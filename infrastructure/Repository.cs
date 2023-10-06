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
        description as {nameof(Box.Description)},
        datetime_created as {nameof(Box.Created)},
        title as {nameof(Box.Title)}
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
        description as {nameof(Box.Description)},
        datetime_created as {nameof(Box.Created)},
        title as {nameof(Box.Title)}

        FROM box_factory.box_inventory
        where guid = @guid";

        using var con = _dataSource.OpenConnection();
        return con.QuerySingle<Box>(sql, new { guid });
    }

    public Box CreateBox(Box box)
    {
        const string sql = @$"INSERT INTO box_factory.box_inventory (title, width, height, depth, location, description) 
        VALUES (@{nameof(Box.Title)}, @{nameof(Box.Width)}, @{nameof(Box.Height)}, @{nameof(Box.Depth)}, @{nameof(Box.Location)}, @{nameof(Box.Description)})
        RETURNING 
            guid as {nameof(Box.Guid)}, 
            width as {nameof(Box.Width)}, 
            height as {nameof(Box.Height)}, 
            depth as {nameof(Box.Depth)}, 
            location as {nameof(Box.Location)}, 
            description as {nameof(Box.Description)},
            datetime_created as {nameof(Box.Created)},
            title as {nameof(Box.Title)}
        ";

        using var con = _dataSource.OpenConnection();
        return con.QuerySingle<Box>(sql, box);
    }

    public Box UpdateBox(Box box)
    {
        const string sql = @$"UPDATE box_factory.box_inventory SET title = @{nameof(Box.Title)},
        width = @{nameof(Box.Width)}, height = @{nameof(Box.Height)}, depth = @{nameof(Box.Depth)}, location = @{nameof(Box.Location)},description = @{nameof(Box.Description)}
        WHERE guid = @{nameof(Box.Guid)}
        RETURNING 
            guid as {nameof(Box.Guid)}, 
            width as {nameof(Box.Width)}, 
            height as {nameof(Box.Height)}, 
            depth as {nameof(Box.Depth)}, 
            location as {nameof(Box.Location)}, 
            description as {nameof(Box.Description)},
            datetime_created as {nameof(Box.Created)},
            title as {nameof(Box.Title)}
            ";

        using var con = _dataSource.OpenConnection();
        return con.QuerySingle<Box>(sql, box);
    }

    public int DeleteBox(Guid guid)
    {
        const string sql = @"DELETE FROM box_factory.box_inventory WHERE guid = @guid";

        using var con = _dataSource.OpenConnection();
        return con.Execute(sql, new { guid });
    }
}