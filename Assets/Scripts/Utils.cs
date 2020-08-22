using UnityEngine;
using UnityEditor;

public static class Utils
{
    public static float GAME_WIDTH = 40f;
    public static float GAME_HEIGHT = 40f;
    public static int FOOD_MAX = 500;
    public static float TIMER = 60f;
    public static float MAX_SIZE = 100f;
    public static float MAX_DIST = 5f;

    public static Color[] colors =
    {
        new Color32(106, 127, 219, 255), //Glaucous
        new Color32(255, 170, 234, 255), //Orchid Crayola
        new Color32(169, 251, 215, 255), //Magic Mint
        new Color32(178, 228, 219, 255), //Middle Blue Green
        new Color32(252, 171, 100, 255), //Rajah
        new Color32(220, 214, 247, 255), //Lavender Web
        new Color32(194, 153, 121, 255), //Antique Brass
        new Color32(232, 106, 146, 255), //Cyclamen
        new Color32(244, 216, 205, 255), //Champagne Pink
        new Color32(247, 175, 157, 255), //Melon
        new Color32(255, 202, 212, 255), //Pink
        new Color32(192, 132, 151, 255), //Puce
        new Color32(192, 155, 216, 255), //Wisteria
        new Color32(67, 146, 241, 255), //Dodger Blue
        new Color32(168, 51, 185, 255), //Purpureus
        new Color32(255, 111, 89, 255), //Bittersweet
        new Color32(80, 132, 132, 255), //Steel Teal
        new Color32(247, 231, 51, 255), //Middle Yellow
        new Color32(27, 154, 170, 255), //Blue Munsell
        new Color32(239, 48, 84, 255), //Red Crayola
        new Color32(225, 85, 84, 255), //Indian Red
        new Color32(195, 49, 73, 255), //French Raspberry
        new Color32(118, 97, 83, 255), //Umber
        new Color32(63, 13, 18, 255), //Dark Sienna
        new Color32(55, 61, 32, 255), //Kombu Green
        new Color32(71, 40, 54, 255), //Eggplant
        new Color32(43, 45, 66, 255), //Space Cadet
        new Color32(77, 83, 89, 255), //Davys Grey
        new Color32(74, 66, 56, 255), //Dark Lava
        new Color32(137, 2, 62, 255), //Claret
        new Color32(43, 65, 98, 255) //Indigo Dye
    };

    //        new Color32(252, 239, 239, 255), //Snow (Not Visible)


    //Rate at which players lose size
    public static float DECAY_RATE = 0.02f;

    public static Color RandomColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }

    public static System.Tuple<int, int> GetCoordinate(Transform transform)
    {
        return GetCoordinate(transform.position);
    }

    public static System.Tuple<int, int> GetCoordinate(Vector3 vector)
    {
        return GetCoordinate(vector.x, vector.y);
    }

    public static System.Tuple<int, int> GetCoordinate(float x, float y)
    {
        return new System.Tuple<int, int>((int)x, (int)y);
    }
}