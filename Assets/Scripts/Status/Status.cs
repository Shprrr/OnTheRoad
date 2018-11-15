using System;

[Serializable]
public abstract class Status : IData
{
    public string _id;
    public string _name;
    public string _description;
    public int TurnLeft;

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }

    public Status()
    {

    }

    public Status(Status status, int turnLeft)
    {
        Id = status.Id;
        Name = status.Name;
        Description = status.Description;
        TurnLeft = turnLeft;
    }

    public virtual bool Stackable(Status other)
    {
        if (Id == other.Id && TurnLeft < other.TurnLeft)
        {
            TurnLeft = other.TurnLeft;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return Name;
    }

    public abstract Status Copy(int turns);
}
