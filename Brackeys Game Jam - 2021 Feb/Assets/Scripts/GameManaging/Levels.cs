
public static class Levels {
	
    public static int numLevels = 12;
    public static bool[] isUnlocked = new bool[numLevels];

    static Levels() {
        isUnlocked[0] = true;
        for (int i = 1; i < numLevels; i++)
            isUnlocked[i] = false;
    }

}
