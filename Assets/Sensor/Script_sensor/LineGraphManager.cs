﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using KyleDulce.SocketIo;


public class LineGraphManager : MonoBehaviour
{
    Socket s;
    public Text hrText;
    public string c;
    //public int name = 0;
    public int index;

    public GameObject linerenderer;
    public GameObject pointer;

    public GameObject pointerRed;
    public GameObject pointerBlue;

    public GameObject HolderPrefb;

    public GameObject holder;
    public GameObject xLineNumber;

    public Material bluemat;
    public Material greenmat;

    public Text topValue;

    public List<GraphData> graphDataPlayer1 = new List<GraphData>();
    public List<GraphData> graphDataPlayer2 = new List<GraphData>();

    private GraphData gd;
    private float highestValue = 56;

    public Transform origin;

    public TextMesh player1name;
    public TextMesh player2name;

    private float lrWidth = 0.1f;
    private int dataGap = 2;
    //int num = 0;

    void call(string d)
    {
        //Debug.Log("RECEIVED EVENT: " + d);
        //s.emit("testEvent1", "test");
        c = d;
    }
    private float fDestroyTime = 2f;
    private float fTickTime = 0;

    void Start()
    {

        //The url must include "ws://" as the protocol
        s = SocketIo.establishSocketConnection("ws://15.164.189.254:58130");
        s.connect();
        s.on("testEvent", call);
        s.on("hr", call);

        // adding random data
        index = 0;

        /*		for (int i = 0; i < index; i++)
                {
                    tim = 0;
                    GraphData gd = new GraphData();
                    gd.marbles = name;
                    graphDataPlayer1.Add(gd);
                    GraphData gd2 = new GraphData();
                    gd2.marbles = Random.Range(10, 47);
                    graphDataPlayer2.Add(gd2);

                    for (; tim < 1; timer += Time.deltaTime)
                    {
                        //Action
                        Debug.Log(timer);
                        Debug.Log(waitingTime);
                        if (timer > waitingTime)
                        {
                            tim = 2;
                            break;
                        }
                    }
                }*/

        // showing graph

    }
    void Update()
    {

        /*		fTickTime += Time.deltaTime;

				if (fTickTime >= fDestroyTime)
				{
					GraphData gd = new GraphData();
					gd.marbles = name;
					graphDataPlayer1.Add(gd);
					GraphData gd2 = new GraphData();
					gd2.marbles = name;
					graphDataPlayer2.Add(gd2);
					ShowGraph();
				}
		*/
        hrText.text = c;
        fTickTime += Time.deltaTime;
        if (fTickTime >= fDestroyTime)
        {
            s.on("testEvent", call);
            //int i = int.TryParse(c, out int i);
            hrText.text = c;
        }
        fTickTime = 0;
        /*		if (i > 0)
                    name = i + 0;
                num++;
                GraphData gd = new GraphData();
                gd.marbles = name;
                Debug.Log(gd.marbles);
                //gd.marbles = num;
                graphDataPlayer1.Add(gd);
                GraphData gd2 = new GraphData();
                gd2.marbles = name;
                //gd2.marbles = num;
                graphDataPlayer2.Add(gd2);
                fTickTime = 0;
                index += 1;
                if (index > 10)
                {
                    graphDataPlayer1.RemoveAt(0);
                    graphDataPlayer2.RemoveAt(0);
                }
                ShowGraph();
                }*/
    }


    public void ShowData(GraphData[] gdlist, int playerNum, float gap)
    {

        // Adjusting value to fit in graph
        for (int i = 0; i < gdlist.Length; i++)
        {
            // since Y axis is from 0 to 7 we are dividing the marbles with the highestValue
            // so that we get a value less than or equals to 1 and than we can multiply that
            // number with Y axis range to fit in graph. 
            // e.g. marbles = 90, highest = 90 so 90/90 = 1 and than 1*7 = 7 so for 90, Y = 7
            gdlist[i].marbles = (gdlist[i].marbles / highestValue) * 7;
        }
        if (playerNum == 1)
            StartCoroutine(BarGraphBlue(gdlist, gap));
        else if (playerNum == 2)
            StartCoroutine(BarGraphGreen(gdlist, gap));
    }

    public void AddPlayer1Data(int numOfStones)
    {
        GraphData gd = new GraphData();
        gd.marbles = numOfStones;
        graphDataPlayer1.Add(gd);
    }
    public void AddPlayer2Data(int numOfStones)
    {
        GraphData gd = new GraphData();
        gd.marbles = numOfStones;
        graphDataPlayer2.Add(gd);
    }

    public void ShowGraph()
    {

        ClearGraph();

        if (graphDataPlayer1.Count >= 1 && graphDataPlayer2.Count >= 1)
        {
            holder = Instantiate(HolderPrefb, Vector3.zero, Quaternion.identity) as GameObject;
            holder.name = "h2";

            GraphData[] gd1 = new GraphData[graphDataPlayer1.Count];
            GraphData[] gd2 = new GraphData[graphDataPlayer2.Count];
            for (int i = 0; i < graphDataPlayer1.Count; i++)
            {
                GraphData gd = new GraphData();
                gd.marbles = graphDataPlayer1[i].marbles;
                gd1[i] = gd;
            }
            for (int i = 0; i < graphDataPlayer2.Count; i++)
            {
                GraphData gd = new GraphData();
                gd.marbles = graphDataPlayer2[i].marbles;
                gd2[i] = gd;
            }

            dataGap = GetDataGap(graphDataPlayer2.Count);


            int dataCount = 0;
            int gapLength = 1;
            float gap = 1.0f;
            bool flag = false;

            while (dataCount < graphDataPlayer2.Count)
            {
                if (dataGap > 1)
                {

                    if ((dataCount + dataGap) == graphDataPlayer2.Count)
                    {

                        dataCount += dataGap - 1;
                        flag = true;
                    }
                    else if ((dataCount + dataGap) > graphDataPlayer2.Count && !flag)
                    {

                        dataCount = graphDataPlayer2.Count - 1;
                        flag = true;
                    }
                    else
                    {
                        dataCount += dataGap;
                        if (dataCount == (graphDataPlayer2.Count - 1))
                            flag = true;
                    }
                }
                else
                    dataCount += dataGap;

                gapLength++;
            }

            if (graphDataPlayer2.Count > 13)
            {
                if (graphDataPlayer2.Count < 40)
                    gap = 13.0f / graphDataPlayer2.Count;
                else if (graphDataPlayer2.Count >= 40)
                {
                    gap = 13.0f / gapLength;
                }
            }

            ShowData(gd1, 1, gap);
            //ShowData(gd2,2,gap);
        }
    }

    public void ClearGraph()
    {
        if (holder)
            Destroy(holder);
    }

    int GetDataGap(int dataCount)
    {
        int value = 1;
        int num = 0;
        while ((dataCount - (100 + num)) >= 0)
        {
            value += 1;
            num += 20;
        }

        return value;
    }


    IEnumerator BarGraphBlue(GraphData[] gd, float gap)
    {
        float xIncrement = gap;
        int dataCount = 0;
        bool flag = false;
        Vector3 startpoint = new Vector3((origin.position.x + xIncrement), (origin.position.y + gd[dataCount].marbles), (origin.position.z));//origin.position;//

        while (dataCount < gd.Length)
        {

            Vector3 endpoint = new Vector3((origin.position.x + xIncrement), (origin.position.y + gd[dataCount].marbles), (origin.position.z));
            startpoint = new Vector3(startpoint.x, startpoint.y, origin.position.z);
            // pointer is an empty gameObject, i made a prefab of it and attach it in the inspector
            GameObject p = Instantiate(pointer, new Vector3(startpoint.x, startpoint.y, origin.position.z), Quaternion.identity) as GameObject;
            p.transform.parent = holder.transform;


            GameObject lineNumber = Instantiate(xLineNumber, new Vector3(origin.position.x + xIncrement, origin.position.y - 0.18f, origin.position.z), Quaternion.identity) as GameObject;
            lineNumber.transform.parent = holder.transform;
            lineNumber.GetComponent<TextMesh>().text = (dataCount + 1).ToString();


            // linerenderer is an empty gameObject with Line Renderer Component Attach to it, 
            // i made a prefab of it and attach it in the inspector
            GameObject lineObj = Instantiate(linerenderer, startpoint, Quaternion.identity) as GameObject;
            lineObj.transform.parent = holder.transform;
            lineObj.name = dataCount.ToString();

            LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

            lineRenderer.material = bluemat;
            lineRenderer.SetWidth(lrWidth, lrWidth);
            lineRenderer.SetVertexCount(2);

            while (Vector3.Distance(p.transform.position, endpoint) > 0.2f)
            {
                float step = 5 * Time.deltaTime;
                p.transform.position = Vector3.MoveTowards(p.transform.position, endpoint, step);
                lineRenderer.SetPosition(0, startpoint);
                lineRenderer.SetPosition(1, p.transform.position);

                yield return null;
            }

            lineRenderer.SetPosition(0, startpoint);
            lineRenderer.SetPosition(1, endpoint);


            p.transform.position = endpoint;
            GameObject pointered = Instantiate(pointerRed, endpoint, pointerRed.transform.rotation) as GameObject;
            pointered.transform.parent = holder.transform;
            startpoint = endpoint;

            if (dataGap > 1)
            {
                if ((dataCount + dataGap) == gd.Length)
                {
                    dataCount += dataGap - 1;
                    flag = true;
                }
                else if ((dataCount + dataGap) > gd.Length && !flag)
                {
                    dataCount = gd.Length - 1;
                    flag = true;
                }
                else
                {
                    dataCount += dataGap;
                    if (dataCount == (gd.Length - 1))
                        flag = true;
                }
            }
            else
                dataCount += dataGap;

            xIncrement += gap;

            yield return null;

        }
    }

    IEnumerator BarGraphGreen(GraphData[] gd, float gap)
    {
        float xIncrement = gap;
        int dataCount = 0;
        bool flag = false;

        Vector3 startpoint = new Vector3((origin.position.x + xIncrement), (origin.position.y + gd[dataCount].marbles), (origin.position.z));
        while (dataCount < gd.Length)
        {

            Vector3 endpoint = new Vector3((origin.position.x + xIncrement), (origin.position.y + gd[dataCount].marbles), (origin.position.z));
            startpoint = new Vector3(startpoint.x, startpoint.y, origin.position.z);
            // pointer is an empty gameObject, i made a prefab of it and attach it in the inspector
            GameObject p = Instantiate(pointer, new Vector3(startpoint.x, startpoint.y, origin.position.z), Quaternion.identity) as GameObject;
            p.transform.parent = holder.transform;

            // linerenderer is an empty gameObject with Line Renderer Component Attach to it, 
            // i made a prefab of it and attach it in the inspector
            GameObject lineObj = Instantiate(linerenderer, startpoint, Quaternion.identity) as GameObject;
            lineObj.transform.parent = holder.transform;
            lineObj.name = dataCount.ToString();

            LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

            lineRenderer.material = greenmat;
            lineRenderer.SetWidth(lrWidth, lrWidth);
            lineRenderer.SetVertexCount(2);

            while (Vector3.Distance(p.transform.position, endpoint) > 0.2f)
            {
                float step = 1;
                p.transform.position = Vector3.MoveTowards(p.transform.position, endpoint, step);
                lineRenderer.SetPosition(0, startpoint);
                lineRenderer.SetPosition(1, p.transform.position);

                yield return null;
            }

            lineRenderer.SetPosition(0, startpoint);
            lineRenderer.SetPosition(1, endpoint);


            p.transform.position = endpoint;
            GameObject pointerblue = Instantiate(pointerBlue, endpoint, pointerBlue.transform.rotation) as GameObject;
            pointerblue.transform.parent = holder.transform;
            startpoint = endpoint;

            if (dataGap > 1)
            {
                if ((dataCount + dataGap) == gd.Length)
                {
                    dataCount += dataGap - 1;
                    flag = true;
                }
                else if ((dataCount + dataGap) > gd.Length && !flag)
                {
                    dataCount = gd.Length - 1;
                    flag = true;
                }
                else
                {
                    dataCount += dataGap;
                    if (dataCount == (gd.Length - 1))
                        flag = true;
                }
            }
            else
                dataCount += dataGap;

            xIncrement += gap;

            yield return null;

        }
    }



    public class GraphData
    {
        public float marbles;
    }
}
