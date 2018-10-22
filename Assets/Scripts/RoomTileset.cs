using System;
using UnityEngine;

[Serializable]
public class RoomTileset
{
    public Sprite noDoor;
    public Sprite leftDoor;
    public Sprite upDoor;
    public Sprite rightDoor;
    public Sprite downDoor;

    public Sprite leftUpDoor;
    public Sprite leftRightDoor;
    public Sprite leftDownDoor;
    public Sprite upRightDoor;
    public Sprite upDownDoor;
    public Sprite rightDownDoor;

    public Sprite leftUpRightDoor;
    public Sprite leftUpDownDoor;
    public Sprite leftRightDownDoor;
    public Sprite upRightDownDoor;
    public Sprite allDoor;
    public Sprite noRoom;
    public Sprite fogOfWar;

    [Header("Indicators")]
    public Sprite currentPosition;
    public Sprite battle;
    public Sprite treasure;

    public Sprite GetDoorSprite(bool hasLeftDoor, bool hasRightDoor, bool hasUpDoor, bool hasDownDoor)
    {
        if (hasLeftDoor)
        {
            if (hasRightDoor)
            {
                if (hasUpDoor)
                    return hasDownDoor ? allDoor : leftUpRightDoor;
                else
                    return hasDownDoor ? leftRightDownDoor : leftRightDoor;
            }
            else
            {
                if (hasUpDoor)
                    return hasDownDoor ? leftUpDownDoor : leftUpDoor;
                else
                    return hasDownDoor ? leftDownDoor : leftDoor;
            }
        }
        else
        {
            if (hasRightDoor)
            {
                if (hasUpDoor)
                    return hasDownDoor ? upRightDownDoor : upRightDoor;
                else
                    return hasDownDoor ? rightDownDoor : rightDoor;
            }
            else
            {
                if (hasUpDoor)
                    return hasDownDoor ? upDownDoor : upDoor;
                else
                    return hasDownDoor ? downDoor : noDoor;
            }
        }
    }
}
