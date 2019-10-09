using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Txt {

    public enum Language {CZ, ENG}
    public static Language language = Language.ENG;

    public static string start;
    public static string restart;
    public static string mlcHle;
    public static string hrejsi;
    public static string dejSem;
    public static string odpustHrichy;
    public static string pojdSi;
    public static string pojdVem;
    public static string nezKohout;
    public static string bouchBac;
    public static string uLavice;
    public static string zPlnaHrdla;
    public static string proKristovu;
    public static string klesaSmyslu;
    public static string pridejDrevoDoOhne;
    public static string dejDitetiHracky;
    public static string zablokujOkno;
    public static string nechSeChytitDitetem;
    public static string ochranDite;
    public static string hardMode;
    public static string klikaCvakla;
    public static string tataVchazi;
    public static string matkuVzkrisil;
    public static string avsakDite;
    public static string veMdlobach;
    public static string kNadram;
    public static string marneHleda;
    public static string diteRve;
    public static string matkaRadostne;
    public static string poledniceSvou;
    public static string madeBy;
    public static string iBodejz;
    public static string zeNa;
    public static string polednici;

    public static void updateTextLanguage()
    {
        switch (language)
        {
            case Language.CZ:
                iBodejz = "I bodejž tě sršeň sám!";
                zeNa = "Že na tebe, nezvedníku,";
                polednici = "Polednici zavolám!";
                start = "Začít hrát";
                restart = "Znovu";
                uLavice = "U lavice dítě stálo";
                zPlnaHrdla = "z plna hrdla křičelo.";
                mlcHle = "Mlč! Hle husar a kočárek,";
                hrejsi = "hrej si! Tu máš kohouta!";
                pojdSi = "Pojď si proň, ty Polednice";
                nezKohout = "Než kohout, vůz i husárek";
                bouchBac = "bouch, bác!letí do kouta.";
                dejSem = "Dej sem dítě!";
                odpustHrichy = "Kriste Pane, odpusť hříchy hříšnici!";
                pojdVem = "pojď, vem si ho, zlostníka!";
                proKristovu = "Pro Kristovu drahou muku!";
                klesaSmyslu = "klesá smyslů zbavena.";
                pridejDrevoDoOhne = "Přilož do kamen";
                dejDitetiHracky = "Dones dítěti hračky";
                zablokujOkno = "Zablokuj okno";
                hardMode = "Nezblázni se do poledne";
                ochranDite = "Ochraň dítě";
                nechSeChytitDitetem = "";
                klikaCvakla = "klika cvakla, dvéře letí";
                tataVchazi = "táta vchází do dveří.";
                veMdlobach = "Ve mdlobách tu matka leží,";
                kNadram = "k ňadrám dítě přimknuté";
                matkuVzkrisil = "matku vzkřísil ještě stěží,";
                avsakDite = "avšak dítě zalknuté.";
                marneHleda = "Marně hledá nepořádné světnice viníka.";
                diteRve = "Dítě řve a řve neustále.";
                matkaRadostne = "Matka radostné slzy vzlyká.";
                poledniceSvou = "Polednice svou lekci nese o dům dále";
                madeBy = "Vytvořil Bezza";
                break;
            case Language.ENG:
                iBodejz = "May a hornet come and sting you!";
                zeNa = "Hush, you naughty little fellow,";
                polednici = "Or the Noonday Witch I'll bring you!";
                start = "Start game";
                restart = "Try again";
                uLavice = "By the bench there stood an infant,";
                zPlnaHrdla = "Screaming, screaming, loud and wild";
                mlcHle = "Hush! Your cart's here, your hussar";
                hrejsi = "look, your cockerel! Go on, play!";
                pojdSi = "Come for him, you Noonday Witch, then!";
                nezKohout = "Crash, bang!Soldier, cock and cart";
                bouchBac = "To the corner fly away.";
                dejSem = "Give that child here!";
                odpustHrichy = "Lord, forgive this sinner's sins, my Saviour dear!";
                pojdVem = "Come and take this pest for me!";
                proKristovu = "For Christ's precious torments! gasping,";
                klesaSmyslu = "she sinks senseless with alarm.";
                pridejDrevoDoOhne = "Add wood to the fireplace";
                dejDitetiHracky = "Bring toys to the child";
                zablokujOkno = "Latch the window";
                ochranDite = "Protect child";
                hardMode = "Don't loose your mind till noon";
                nechSeChytitDitetem = "";
                klikaCvakla = "The handle clicks, and as the door";
                tataVchazi = "Flies wide open, father's here.";
                veMdlobach = "Child clasped to her breast, he found,";
                kNadram = "Lying in a faint, the mother";
                matkuVzkrisil = "He could hardly bring her round,";
                avsakDite = "But the little one was smothered.";
                marneHleda = "The culprit of this mess cannot be found.";
                diteRve = "Screaming child seems perplexed.";
                matkaRadostne = "Relieved mother cries on the ground.";
                poledniceSvou = "Who knows to whom the Noonwitch brings her lesson next.";
                madeBy = "Made by Bezza";
                break;
        }
    }
}
