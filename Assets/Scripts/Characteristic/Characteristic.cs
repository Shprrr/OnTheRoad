using System;

[Serializable]
public class Characteristic : IData
{
    public string _id;
    public string _name;
    public string _description;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }

    public IBaseValueStrategy BaseValueStrategy;
    public IMinMaxStrategy MinMaxStrategy;

    public Characteristic(string id, string name, string description, IBaseValueStrategy baseValueStrategy, IMinMaxStrategy minMaxStrategy)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        BaseValueStrategy = baseValueStrategy ?? throw new ArgumentNullException(nameof(baseValueStrategy));
        MinMaxStrategy = minMaxStrategy ?? throw new ArgumentNullException(nameof(minMaxStrategy));
    }

    public override string ToString()
    {
        return Id;
    }
}
