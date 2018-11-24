using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Txt {

    public enum Language {CZ, ENG}
    public static Language language = Language.CZ;

    public static string uLavice;
    public static string zPlnaHrdla;
    public static string pridejDrevoDoOhne;
    public static string dejDitetiHracky;
    public static string zablokujOkno;
    public static string nechSeChytitDitetem;
    public static string hardMode;

    public static void updateTextLanguage()
    {
        switch (language)
        {
            case Language.CZ:
                uLavice = "U lavice dítě stálo,";
                zPlnaHrdla = "z plna hrdla křičelo.";
                pridejDrevoDoOhne = "Přilož do kamen";
                dejDitetiHracky = "Dones dítěti hračky";
                zablokujOkno = "Zablokuj okno";
                hardMode = "Nezblázni se do poledne";
                nechSeChytitDitetem = "-";
                break;
            case Language.ENG:
                uLavice = "By the bench there stood an infant,";
                zPlnaHrdla = "Screaming, screaming, loud and wild;";
                pridejDrevoDoOhne = "Add wood to the fireplace";
                dejDitetiHracky = "Bring toys to the child";
                zablokujOkno = "Latch the window";
                hardMode = "Don't loose your mind till noon";
                nechSeChytitDitetem = "-";
                break;
        }
    }
}
