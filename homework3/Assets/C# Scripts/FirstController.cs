using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController, UserAction
{
    UserGUI userGUI;

    public CoastController startCoast;
    public CoastController endCoast;
    public BoatController boat;
    private MyCharacterController[] characters;

    private int currentCount = 60;
    private System.Timers.Timer timer = new System.Timers.Timer(1000);

    private FirstSceneActionManager actionManager;

    void Start()
    {
        actionManager = GetComponent<FirstSceneActionManager>();
    }

    void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new MyCharacterController[6];
        loadResources();
    }

    public void loadResources()
    {
        GameObject water = Instantiate(Resources.Load("Perfabs/Water", typeof(GameObject)), new Vector3(0, 0.5F, 0), Quaternion.identity, null) as GameObject;
        water.name = "water";
        startCoast = new CoastController("start");
        endCoast = new CoastController("end");
        boat = new BoatController();
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(count);
        timer.Start();
        loadCharacter();
    }

    private void loadCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            MyCharacterController cha = new MyCharacterController("priest" + i, 0);
            cha.setPosition(startCoast.getEmptyPosition());
            cha.getOnCoast(startCoast);
            startCoast.getOnCoast(cha);
            characters[i] = cha;
        }
        for (int i = 0; i < 3; i++)
        {
            MyCharacterController cha = new MyCharacterController("devil" + i, 1);
            cha.setPosition(startCoast.getEmptyPosition());
            cha.getOnCoast(startCoast);
            startCoast.getOnCoast(cha);
            characters[i + 3] = cha;
        }
    }

    public void count(object sender, System.Timers.ElapsedEventArgs e)
    {
        userGUI.setCount(currentCount);
        if (currentCount == 0)
            timer.Stop();
        currentCount--;
    }
    
    public void moveBoat()
    {
        if (boat.isEmpty() || userGUI.status > 0)
            return;
        actionManager.moveBoat(boat);
        boat.move();
        userGUI.status = check_game_over();
    }

    public void characterIsClicked(MyCharacterController characterCtrl)
    {
        if (userGUI.status > 0)
            return;
        if (characterCtrl.isOnBoat())
        {
            CoastController coast = boat.getStatus() == -1 ? endCoast : startCoast;
            boat.GetOffBoat(characterCtrl.getName());
            actionManager.moveCharacter(characterCtrl, coast.getEmptyPosition());
            characterCtrl.getOnCoast(coast);
            coast.getOnCoast(characterCtrl);
        }
        else
        {
            if (boat.getEmptyIndex() == -1)
                return;
            CoastController coast = characterCtrl.getCoastController();
            if (coast.getType() != boat.getStatus())
                return;
            coast.getOffCoast(characterCtrl.getName());
            actionManager.moveCharacter(characterCtrl, boat.getEmptyPosition());
            characterCtrl.getOnBoat(boat);
            boat.GetOnBoat(characterCtrl);
        }
        userGUI.status = check_game_over();
    }

    int check_game_over()
    {
        int start_priest = 0;
        int start_devil = 0;
        int end_priest = 0;
        int end_devil = 0;

        int[] startCount = startCoast.getPassengersType();
        start_priest += startCount[0];
        start_devil += startCount[1];

        int[] endCount = endCoast.getPassengersType();
        end_priest += endCount[0];
        end_devil += endCount[1];

        if (end_priest + end_devil == 6)
        {
            timer.Stop();
            return 2;
        }

        int[] boatCount = boat.getPassengersType();
        if (boat.getStatus() == -1)
        {
            end_priest += boatCount[0];
            end_devil += boatCount[1];
        }
        else
        {
            start_priest += boatCount[0];
            start_devil += boatCount[1];
        }
        if (start_priest < start_devil && start_priest > 0 || end_priest < end_devil && end_priest > 0)
        {
            timer.Stop();
            return 1;
        }
        return 0;
    }

    public void restart()
    {
        currentCount = 60;
        userGUI.setCount(currentCount);
        timer.Start();
        boat.reset();
        startCoast.reset();
        endCoast.reset();
        for (int i = 0; i < characters.Length; i++)
            characters[i].reset();
    }
}
