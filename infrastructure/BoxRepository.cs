using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure;

public class BoxRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public BoxRepository(NpgsqlDataSource dataSource)
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
        title as {nameof(Box.Title)}, 
        quantity as {nameof(Box.Quantity)},
        material_id as {nameof(Box.Material.Id)},
        material_name as {nameof(Box.Material.Name)}
        FROM box_factory.box_inventory
        INNER JOIN box_factory.materials m on m.id = box_inventory.material_id";

        using var con = _dataSource.OpenConnection();
        return con.Query<Box, Material, Box>(sql, (box, material) =>
        {
            box.Material = material;
            return box;
        });
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
        title as {nameof(Box.Title)}, 
        quantity as {nameof(Box.Quantity)},
        material_id as {nameof(Box.Material.Id)},
        material_name as {nameof(Box.Material.Name)}
        FROM box_factory.box_inventory
        INNER JOIN box_factory.materials m on m.id = box_inventory.material_id        
        where guid = @guid";

        using var con = _dataSource.OpenConnection();
        return con.Query<Box, Material, Box>(sql, (box, material) =>
        {
            box.Material = material;
            return box;
        },new { guid }).First();
    }

    public BoxWithMaterialId CreateBox(BoxWithMaterialId box)
    {
        const string sql = @$"INSERT INTO box_factory.box_inventory (title, width, height, depth, location, description, quantity, material_id) 
        VALUES (@{nameof(BoxWithMaterialId.Title)}, @{nameof(BoxWithMaterialId.Width)}, @{nameof(BoxWithMaterialId.Height)}, @{nameof(BoxWithMaterialId.Depth)}, @{nameof(BoxWithMaterialId.Location)}, @{nameof(BoxWithMaterialId.Description)}, @{nameof(BoxWithMaterialId.Quantity)}, @{nameof(BoxWithMaterialId.MaterialId)})
        RETURNING 
            guid as {nameof(BoxWithMaterialId.Guid)}, 
            width as {nameof(BoxWithMaterialId.Width)}, 
            height as {nameof(BoxWithMaterialId.Height)}, 
            depth as {nameof(BoxWithMaterialId.Depth)}, 
            location as {nameof(BoxWithMaterialId.Location)}, 
            description as {nameof(BoxWithMaterialId.Description)},
            datetime_created as {nameof(BoxWithMaterialId.Created)},
            title as {nameof(BoxWithMaterialId.Title)},
            quantity as {nameof(BoxWithMaterialId.Quantity)},
            material_id as {nameof(BoxWithMaterialId.MaterialId)}
        ";

        using var con = _dataSource.OpenConnection();
        return con.QueryFirst<BoxWithMaterialId>(sql, box);
    }

    public BoxWithMaterialId UpdateBox(Box box)
    {
        const string sql = @$"UPDATE box_factory.box_inventory SET title = @{nameof(Box.Title)},
        width = @{nameof(Box.Width)}, height = @{nameof(Box.Height)}, depth = @{nameof(Box.Depth)}, location = @{nameof(Box.Location)},description = @{nameof(Box.Description)}, quantity = @{nameof(Box.Quantity)}, material_id = @{nameof(Box.Material.Id)}
        WHERE guid = @{nameof(Box.Guid)}
        RETURNING 
            guid as {nameof(Box.Guid)}, 
            width as {nameof(Box.Width)}, 
            height as {nameof(Box.Height)}, 
            depth as {nameof(Box.Depth)}, 
            location as {nameof(Box.Location)}, 
            description as {nameof(Box.Description)},
            datetime_created as {nameof(Box.Created)},
            title as {nameof(Box.Title)}, 
            quantity as {nameof(Box.Quantity)},
            material_id as {nameof(Box.Material.Id)},
            ";

        using var con = _dataSource.OpenConnection();
        return con.QuerySingle<BoxWithMaterialId>(sql, box);
    }

    public int DeleteBox(Guid guid)
    {
        const string sql = @"DELETE FROM box_factory.box_inventory WHERE guid = @guid";

        using var con = _dataSource.OpenConnection();
        return con.Execute(sql, new { guid });
    }

    public IEnumerable<BoxOverviewItem> GetFeed()
    {
       const string sql = @$"SELECT 
        guid as {nameof(BoxOverviewItem.Guid)},
        width as {nameof(BoxOverviewItem.Width)},
        height as {nameof(BoxOverviewItem.Height)},
        depth as {nameof(BoxOverviewItem.Depth)},
        title as {nameof(BoxOverviewItem.Title)}, 
        quantity as {nameof(BoxOverviewItem.Quantity)},
        material_name as {nameof(BoxOverviewItem.MaterialName)}
        FROM box_factory.box_inventory
        INNER JOIN box_factory.materials m on m.id = box_inventory.material_id";

        using var con = _dataSource.OpenConnection();
        return con.Query<BoxOverviewItem>(sql);
    }
}