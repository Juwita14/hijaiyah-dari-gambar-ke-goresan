public class Konstanta
{
    public static float PANEL_TWEEN_DURATION = .5f;
    public static int OUTPUT_SIZE = 11;
    private static string[] hurufs = { "ba", "tsa", "jim", "kha", "sin", "shad", "tha", "qaf", "kaf", "nun", "hamzah" };
    private static string[] realhurufs = { "alif","ba","ta","tsa","jim","ha'","kha","dal","dzal","ra","za",
                                            "sin","syin","shad","dhad","tha","zha","ain","ghain","fa","qaf",
                                            "kaf","lam","mim","nun","waw","ha","lamalif","hamzah","ya"};

    static string[] arab_datasets={"ain","alif","ba","dal","dhad","dzal","fa","ghain",
                  "ha", "ha'","hamzah","jim","kaf","kha","lam","lamalif",
                  "mim","nun","qaf","ra","shad","sin","syin","ta","tha",
                  "tsa","waw","ya","za","zha"};

    static int[] arab_datasets_index={17,0,1,7,14,8,19,18,26,5,28,4,21,6,22,27,23,24,20,9,13,11,12,2,15,3,25,29,10,16};

    private static string[] arabicHurufs = {"ا", "ب","ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز",
                                            "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق",
                                            "ك", "ل", "م", "ن", "و", "ھ", "لا", "ء", "ي"};
    public static int REAL_OUTPUT_SIZE = 30;
    public static string GetRealHuruf(int index) {
        return realhurufs[index];
    }

    public static string GetDatasetHuruf(int index) {
        return arab_datasets[index];
    }

    public static int ConvertPredictionToRealIndex(int _predictionIndex) {
        int idx = arab_datasets_index[_predictionIndex];
        return idx;
    }

    public static string GetArabicHuruf(int index) {
        return arabicHurufs[index];
    }

    public static string GetHuruf(int index) { 
        return hurufs[index];
    }
}