using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    int _width, _height;

    Spawner[] topWeapons;
    Spawner[] bottomWeapons;
    Spawner[] leftWeapons;
    Spawner[] rightWeapons;

    enum weaponSide {Top, Bottom, Left, Right}

    [SerializeField]
    Spawner.weapon weaponMode = Spawner.weapon.boulder;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WaveSpamRandomSide(10, 0.5f));
        }
    }

    ////////////////////////////////////////////////////////////
    ///attack decision logic?




    /////////////////////////////////////////////////////////////
    ///Coprocesses to spawn specific attacks
    ///
    /// Single wave of shots from random side
    IEnumerator WaveSingleOneSide(int amount)
    {
        weaponSide ws = chooseSingleSide();
        fireOneSide(getSide(ws), amount);
        yield return null;
    }

    //multiple successive waves from a single side
    IEnumerator WavesMultipleOneSide(int[] amounts, float delay)
    {
        weaponSide ws = chooseSingleSide();
        foreach (int i in amounts)
        {
            fireOneSide(getSide(ws), i);
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    //Single wave shooting from opposite sides
    IEnumerator WaveSingleOppositeSides(int amount)
    {
        weaponSide ws = chooseSingleSide();
        fireOppositeSides(getSide(ws), getOppositeSide(ws), amount);
        yield return null;
    }

    //multiple successive waves from opposite sides
    IEnumerator WavesMultipleOppositeSides(int[] amounts, float delay)
    {
        weaponSide ws = chooseSingleSide();
        foreach (int i in amounts)
        {
            fireOppositeSides(getSide(ws), getOppositeSide(ws), i);
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    //Single wave shooting from perpendicular sides
    IEnumerator WaveSinglePerpendicularSides(int amount, int amountPerpendicular)
    {
        weaponSide ws = chooseSingleSide();
        fireOneSide(getSide(ws), amount);
        fireOneSide(getPerpendicularSide(ws), amountPerpendicular);
        yield return null;
    }

    //multiple successive waves from perpendicular sides (note perpendicular side will flip randomly +/- 90 degrees)
    //make sure amount arrays are same size
    IEnumerator WavesMultiplePerpendicularSides(int[] amounts, int[] amountsPerpendicular, float delay)
    {
        if (amounts.Length == amountsPerpendicular.Length)
        {
            weaponSide ws = chooseSingleSide();
            int amountPerpendicularIterator = 0;
            foreach (int i in amounts)
            {
                fireOneSide(getSide(ws), i);
                fireOneSide(getPerpendicularSide(ws), amountsPerpendicular[amountPerpendicularIterator]);
                amountPerpendicularIterator++;
                yield return new WaitForSeconds(delay);
            }            
        }
        yield return null;
    }

    //Single Wave from 3 sides simultaneously (main + opposite + perpendicular)
    IEnumerator WaveSingleThreeSides(int amountOpposites, int amountPerpendicular)
    {
        weaponSide ws = chooseSingleSide();
        fireOppositeSides(getSide(ws), getOppositeSide(ws), amountOpposites);
        fireOneSide(getPerpendicularSide(ws), amountPerpendicular);
        yield return null;
    }

    //multiple waves from 3 sides (main + opposite + perpendicular)
    //note: perpendicular side will flip randomly
    IEnumerator WavesMultipleThreeSides(int[] amountsOpposite, int[] amountsPerpendicular, float delay)
    {
        if (amountsOpposite.Length == amountsPerpendicular.Length)
        {
            weaponSide ws = chooseSingleSide();
            int amountPerpendicularIterator = 0;
            foreach (int i in amountsOpposite)
            {
                fireOppositeSides(getSide(ws), getOppositeSide(ws), i);
                fireOneSide(getPerpendicularSide(ws), amountsPerpendicular[amountPerpendicularIterator]);
                amountPerpendicularIterator++;
                yield return new WaitForSeconds(delay);
            }
        }
        yield return null;
    }

    //Single Wave from all 4 sides
    IEnumerator WaveSingleFourSides(int amountTop, int amountBottom, int amountLeft, int amountRight)
    {
        fireOneSide(getSide(weaponSide.Top), amountTop);
        fireOneSide(getSide(weaponSide.Bottom), amountBottom);
        fireOneSide(getSide(weaponSide.Left), amountLeft);
        fireOneSide(getSide(weaponSide.Right), amountRight);
        yield return null;
    }

    //Multiple successive waves from every side at once
    //multiple Waves from all 4 sides
    IEnumerator WaveMultipleFourSides(int[] amountsTop, int[] amountsBottom, int[] amountsLeft, int[] amountsRight, float delay)
    {
        if (amountsTop.Length == amountsBottom.Length && amountsBottom.Length == amountsLeft.Length && amountsLeft == amountsRight)
        {
            for (int i = 0; i < amountsTop.Length; i++)
            {
                fireOneSide(getSide(weaponSide.Top), amountsTop[i]);
                fireOneSide(getSide(weaponSide.Bottom), amountsBottom[i]);
                fireOneSide(getSide(weaponSide.Left), amountsLeft[i]);
                fireOneSide(getSide(weaponSide.Right), amountsRight[i]);
                yield return new WaitForSeconds(delay); ;
            }
        }
        yield return null;
    }

    //spams single weapon attacks from any side
    IEnumerator WaveSpamRandomSide(int spamCount, float delay)
    {
        while (spamCount > 0)
        {
            StartCoroutine(WaveSingleOneSide(1));
            spamCount--;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }







    /////////////////////////////////////////////////////////////////////////////////////////////
    //main weapon firing functionality, coprocesses will activate these depending on need
    //This is the main firing function takes an array of weapon types and fires them for the given side
    public void fireSpecificWeaponsOnSide(Spawner[] side, Spawner.weapon[] weaponsToFire)
    {
        for(int i = 0; i < side.Length; i++)
        {
            side[i].spawnAttack(weaponsToFire[i]);
        }
    }

    //Logic to fire only one side of weapons, amount dictates number of weapon activations
    public void fireOneSide(Spawner[] side, int amount)
    {
        List<int> weaponsToFire = chooseRandomWeaponPositions(amount, side.Length);
        Spawner.weapon[] weaponArray = getBlankWeaponArray(side.Length);
        foreach(int pos in weaponsToFire)
        {
            weaponArray[pos] = getCurrentWeapon();
        }
        fireSpecificWeaponsOnSide(side, weaponArray);
    }

    //fire two sides simultaneously, amount is split randomly between the 2 (could all go to one side)
    public void fireOppositeSides(Spawner[] side1, Spawner[] side2, int amount)
    {
        List<int> weaponsToFire = chooseRandomWeaponPositions(amount, side1.Length);
        Spawner.weapon[] weaponArray1 = getBlankWeaponArray(side1.Length);
        Spawner.weapon[] weaponArray2 = getBlankWeaponArray(side2.Length);
        foreach (int pos in weaponsToFire)
        {
            if(1 == Random.Range(0,2))
            {
                weaponArray1[pos] = getCurrentWeapon();
            }
            else
            {
                weaponArray2[pos] = getCurrentWeapon();
            } 
        }
        fireSpecificWeaponsOnSide(side1, weaponArray1);
        fireSpecificWeaponsOnSide(side2, weaponArray2);
    }

    //helper function to randomly select which weapon positions will fire
    List<int> chooseRandomWeaponPositions(int amountToChose, int sideLength)
    {
        List<int> possiblePositions = new List<int>();
        for (int i = 0; i < sideLength; i++)
        {
            possiblePositions.Add(i);
        }
        while (possiblePositions.Count > amountToChose)
        {
            possiblePositions.RemoveAt(Random.Range(0, possiblePositions.Count));
        }
        return possiblePositions;
    }

    //returns either the currently selected weapon or a random one if set to none
    Spawner.weapon getCurrentWeapon()
    {
        if (weaponMode == Spawner.weapon.none)
        {
            return (Spawner.weapon)Random.Range(0, (int)Spawner.weapon.none);
        }
        else
        {
            return weaponMode;
        }
    }

    //helper function to initialize a blank (none) weapon array
    Spawner.weapon[] getBlankWeaponArray(int amount)
    {
        Spawner.weapon[] weaponArray = new Spawner.weapon[amount];
        for(int i = 0; i < amount; i++)
        {
            weaponArray[i] = Spawner.weapon.none;
        }
        return weaponArray;
    }

    //helper function to choose a random single side
    weaponSide chooseSingleSide()
    {
        return (weaponSide)Random.Range(0, (int)weaponSide.Right + 1);
    }

    //helper function to get the array for a given side
    Spawner[] getSide(weaponSide ws)
    {
        switch (ws)
        {
            case weaponSide.Top:
                return topWeapons;
            case weaponSide.Bottom:
                return bottomWeapons;
            case weaponSide.Left:
                return leftWeapons;
            case weaponSide.Right:
                return rightWeapons;
            default:
                return topWeapons;
        }
    }

    //helper function to get the array of the opposite side of given side
    Spawner[] getOppositeSide(weaponSide ws)
    {
        switch (ws)
        {
            case weaponSide.Top:
                return bottomWeapons;
            case weaponSide.Bottom:
                return topWeapons;
            case weaponSide.Left:
                return rightWeapons;
            case weaponSide.Right:
                return leftWeapons;
            default:
                return topWeapons;
        }
    }


    //Helper function to get a side array 90 degrees to left or right of given side
    //helper function to get the array of the opposite side of given side
    Spawner[] getPerpendicularSide(weaponSide ws)
    {
        weaponSide perpendicularSide;

        if (ws == weaponSide.Bottom || ws == weaponSide.Top)
        {
            perpendicularSide = (1 == Random.Range(0, 2)) ?  weaponSide.Left  : weaponSide.Right;
        }
        else
        {
            perpendicularSide = (1 == Random.Range(0, 2)) ? weaponSide.Top : weaponSide.Bottom;
        }

        return getSide(perpendicularSide);
    }


    /////////////////////////////////////////////////////////////////////////
    //Setup Functions - set dimensions of level and add Spawners to arrays
    public void setDimensions(int w, int h)
    {
        _width = w;
        _height = h;

        topWeapons = new Spawner[w];
        bottomWeapons = new Spawner[w];
        rightWeapons = new Spawner[h];
        leftWeapons = new Spawner[h];
    }

    public void addTop(Spawner S, int pos)
    {
        topWeapons[pos] = S;
    }

    public void addBottom(Spawner S, int pos)
    {
        bottomWeapons[pos] = S;
    }

    public void addRight(Spawner S, int pos)
    {
        rightWeapons[pos] = S;
    }

    public void addLeft(Spawner S, int pos)
    {
        leftWeapons[pos] = S;
    }

}
