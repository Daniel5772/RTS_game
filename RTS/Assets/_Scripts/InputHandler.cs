using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Units.Player;


namespace RTS.InputManager
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;

        private RaycastHit hit; //what we hit with our ray

        private List<Transform> selectedUnits = new List<Transform>();

        private bool isDragging = false;

        private Vector3 mousePos;

        void Start()
        {
            instance = this;
        }

        private void OnGUI()
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f));
                MultiSelect.DrawScreenRectBorder(rect, 3, Color.black);
            }
        }


        public void HandleUnitMovement()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
                //create a ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //check if we hit sth
                if (Physics.Raycast(ray, out hit))
                {
                    //if we do, then do something with that data
                    LayerMask layerHit = hit.transform.gameObject.layer;

                    switch (layerHit.value)
                    {
                        case 8: //Units Layer
                            //do sth
                            SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
                            break;
                        default: //if none of the above happen
                            //do sth
                            isDragging = true;
                            DeselectUnits();
                            break;

                    }
                }

            }

            if (Input.GetMouseButtonUp(0))
            {

                foreach (Transform child in Player.PlayerManager.instance.playerUnits)
                {
                    foreach (Transform unit in child)
                    {
                        if (isWithinSelectionBounds(unit))
                        {
                            SelectUnit(unit, true);
                        }
                    }
                }
                isDragging = false;
            }

            if (Input.GetMouseButtonDown(1) && HaveSelectedUnits())
            {
                //create a ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //check if we hit sth
                if (Physics.Raycast(ray, out hit))
                {
                    //if we do, then do something with that data
                    LayerMask layerHit = hit.transform.gameObject.layer;

                    switch (layerHit.value)
                    {
                        case 8: //Units Layer
                            //do sth
                            break;
                        case 9: //enemy units layer
                            //attack or set target
                        default: //if none of the above happen
                            //do sth
                            foreach (Transform unit in selectedUnits)
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                pU.MoveUnit(hit.point);
                            }
                            break;

                    }
                }
            }
        }
        private void SelectUnit(Transform unit, bool canMultiselect = false)
        {
            if (!canMultiselect)
            {
                DeselectUnits();
            }
            selectedUnits.Add(unit);
            //lets set an obj on that unit called Highlight
            unit.Find("Highlight").gameObject.SetActive(true);
        }

        private void DeselectUnits()
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].Find("Highlight").gameObject.SetActive(false);
            }
            selectedUnits.Clear();
        }
        private bool isWithinSelectionBounds(Transform tf)
        {
            if (!isDragging)
            {
                return false;
            }

            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVPBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(tf.position));
        }

        private bool HaveSelectedUnits()
        {
            if (selectedUnits.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
