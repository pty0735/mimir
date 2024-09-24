using Lib9c.Models.Items;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Mimir.MongoDB.Bson.Serialization.Serializers.Lib9c.Items;

public class ItemUsableSerializer : ClassSerializerBase<ItemUsable>
{
    public static readonly ItemUsableSerializer Instance = new();

    public static ItemUsable Deserialize(BsonDocument doc)
    {
        if (!doc.TryGetValue("ItemType", out var itemTypeValue))
        {
            throw new BsonSerializationException("Missing ItemType in document.");
        }

        if (!doc.TryGetValue("ItemSubType", out var itemSubTypeValue))
        {
            throw new BsonSerializationException("Missing itemSubTypeValue in document.");
        }

        var itemType = (Nekoyume.Model.Item.ItemType)itemTypeValue.AsInt32;
        var itemSubType = (Nekoyume.Model.Item.ItemSubType)itemSubTypeValue.AsInt32;
        switch (itemType)
        {
            case Nekoyume.Model.Item.ItemType.Consumable:
                return ConsumableSerializer.Deserialize(doc);
            case Nekoyume.Model.Item.ItemType.Equipment:
                return EquipmentSerializer.Deserialize(doc);
        }

        throw new BsonSerializationException($"Unsupported ItemType: {itemType} or ItemSubType: {itemSubType}");
    }

    public override ItemUsable Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var doc = BsonDocumentSerializer.Instance.Deserialize(context, args);
        return Deserialize(doc);
    }

    // DO NOT OVERRIDE Serialize METHOD: Currently objects will be serialized to Json first.
    // public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ItemUsable value)
    // {
    //     base.Serialize(context, args, value);
    // }
}
