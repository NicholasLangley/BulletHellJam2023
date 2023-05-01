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

    enum waveType {OneSide,
        OppositeSides,
        Perpendicular, 
        ThreeSides,
        FourSides,
        OneSideSpam,
        Count}

    [SerializeField]
    Spawner.weapon weaponMode = Spawner.weapon.boulder;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    ////////////////////////////////////////////////////////////
    ///attack decision logic?
    //////
    ////For now at least just random
    ///
    IEnumerator randomWaveGenerator()
    {
        while (true)
        {
            int waveCount = Random.Range(1, 6);
            float betweenWaveDelay = Random.Range(0.6f, 1f);
            float beforeNextAttackDelay = Random.Range(0.0f, 1.0f);
            float chargeTime = Random.Range(0.5f, 1.0f);
            switch (generateRandomWaveType())
            {
                case waveType.OneSide:
                    yield return StartCoroutine(WavesOneSide(generateRandomWaveAmountArray(waveCount), betweenWaveDelay, chargeTime));
                    break;
                case waveType.OppositeSides:
                    yield return StartCoroutine(WavesOppositeSides(generateRandomWaveAmountArray(waveCount), betweenWaveDelay, chargeTime));
                    break;
                case waveType.Perpendicular:
                    yield return StartCoroutine(WavesPerpendicularSides(generateRandomWaveAmountArray(waveCount), generateRandomWaveAmountArray(waveCount),  betweenWaveDelay, chargeTime));
                    break;
                case waveType.ThreeSides:
                    yield return StartCoroutine(WavesThreeSides(generateRandomWaveAmountArray(waveCount), generateRandomWaveAmountArray(waveCount), betweenWaveDelay, chargeTime));
                    break;
                case waveType.FourSides:
                    yield return StartCoroutine(WavesFourSides(generateRandomWaveAmountArray(waveCount), generateRandomWaveAmountArray(waveCount), generateRandomWaveAmountArray(waveCount), generateRandomWaveAmountArray(waveCount), betweenWaveDelay, chargeTime));
                    break;
                case waveType.OneSideSpam:
                    yield return StartCoroutine(WaveSpamRandomSide(waveCount, betweenWaveDelay / 6f, chargeTime));
                    break;
            }
            yield return new WaitForSeconds(beforeNextAttackDelay + chargeTime);
        }
        yield return null;
    }

    //helper function to return a random wave type enum
    waveType generateRandomWaveType()
    {
        return (waveType)Random.Range(0, (int)waveType.Count);
    }

    int[] generateRandomWaveAmountArray(int waveCount)
    {
        int[] waveAmountArray = new int[waveCount];
        for(int i = 0; i < waveCount; i++)
        {
            waveAmountArray[i] = Random.Range(1, 6);
        }
        return waveAmountArray;
    }

    //starts firing of coporocess
    public void startFiring()
    {
        StartCoroutine(randomWaveGenerator());
    }

    /////////////////////////////////////////////////////////////
    ///Coprocesses to spawn specific attacks
    ///


    //wave(s) from a single side
    IEnumerator WavesOneSide(int[] amounts, float delay, float chargeTime)
    {
        weaponSide ws = chooseSingleSide();
        foreach (int i in amounts)
        {
            fireOneSide(getSide(ws), i, chargeTime);
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    //wave(s) from opposite sides
    IEnumerator WavesOppositeSides(int[] amounts, float delay, float chargeTime)
    {
        weaponSide ws = chooseSingleSide();
        foreach (int i in amounts)
        {
            fireOppositeSides(getSide(ws), getOppositeSide(ws), i, chargeTime);
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    //wave(s) from perpendicular sides (note perpendicular side will flip randomly +/- 90 degrees)
    //make sure amount arrays are same size
    IEnumerator WavesPerpendicularSides(int[] amounts, int[] amountsPerpendicular, float delay, float chargeTime)
    {
        if (amounts.Length == amountsPerpendicular.Length)
        {
            weaponSide ws = chooseSingleSide();
            int amountPerpendicularIterator = 0;
            foreach (int i in amounts)
            {
                fireOneSide(getSide(ws), i, chargeTime);
                fireOneSide(getPerpendicularSide(ws), amountsPerpendicular[amountPerpendicularIterator], chargeTime);
                amountPerpendicularIterator++;
                yield return new WaitForSeconds(delay);
            }            
        }
        yield return null;
    }

    //wave(s) from 3 sides (main + opposite + perpendicular)
    //note: perpendicular side will flip randomly
    IEnumerator WavesThreeSides(int[] amountsOpposite, int[] amountsPerpendicular, float delay, float chargeTime)
    {
        if (amountsOpposite.Length == amountsPerpendicular.Length)
        {
            weaponSide ws = chooseSingleSide();
            int amountPerpendicularIterator = 0;
            foreach (int i in amountsOpposite)
            {
                fireOppositeSides(getSide(ws), getOppositeSide(ws), i, chargeTime);
                fireOneSide(getPerpendicularSide(ws), amountsPerpendicular[amountPerpendicularIterator], chargeTime);
                amountPerpendicularIterator++;
                yield return new WaitForSeconds(delay);
            }
        }
        yield return null;
    }

    //Wave(s) from all 4 sides
    IEnumerator WavesFourSides(int[] amountsTop, int[] amountsBottom, int[] amountsLeft, int[] amountsRight, float delay, float chargeTime)
    {
        if (amountsTop.Length == amountsBottom.Length && amountsBottom.Length == amountsLeft.Length && amountsLeft == amountsRight)
        {
            for (int i = 0; i < amountsTop.Length; i++)
            {
                fireOneSide(getSide(weaponSide.Top), amountsTop[i], chargeTime);
                fireOneSide(getSide(weaponSide.Bottom), amountsBottom[i], chargeTime);
                fireOneSide(getSide(weaponSide.Left), amountsLeft[i], chargeTime);
                fireOneSide(getSide(weaponSide.Right), amountsRight[i], chargeTime);
                yield return new WaitForSeconds(delay);
            }
        }
        yield return null;
    }

    //spams single weapon attacks from any side
    IEnumerator WaveSpamRandomSide(int spamCount, float delay, float chargeTime)
    {
        while (spamCount > 0)
        {
            StartCoroutine(WavesOneSide(new int[1] { 1 }, 0.1f, chargeTime));
            spamCount--;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }







    /////////////////////////////////////////////////////////////////////////////////////////////
    //main weapon firing functionality, coprocesses will activate these depending on need
    //This is the main firing function takes an array of weapon types and fires them for the given side
    public void fireSpecificWeaponsOnSide(Spawner[] side, Spawner.weapon[] weaponsToFire, float chargeTime)
    {
        for(int i = 0; i < side.Length; i++)
        {
            side[i].fireWeapon(weaponsToFire[i], chargeTime);
        }
    }

    //Logic to fire only one side of weapons, amount dictates number of weapon activations
    public void fireOneSide(Spawner[] side, int amount, float chargeTime)
    {
        List<int> weaponsToFire = chooseRandomWeaponPositions(amount, side.Length);
        Spawner.weapon[] weaponArray = getBlankWeaponArray(side.Length);
        foreach(int pos in weaponsToFire)
        {
            weaponArray[pos] = getCurrentWeapon();
        }
        fireSpecificWeaponsOnSide(side, weaponArray, chargeTime);
    }

    //fire two sides simultaneously, amount is split randomly between the 2 (could all go to one side)
    public void fireOppositeSides(Spawner[] side1, Spawner[] side2, int amount, float chargeTime)
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
        fireSpecificWeaponsOnSide(side1, weaponArray1, chargeTime);
        fireSpecificWeaponsOnSide(side2, weaponArray2, chargeTime);
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
