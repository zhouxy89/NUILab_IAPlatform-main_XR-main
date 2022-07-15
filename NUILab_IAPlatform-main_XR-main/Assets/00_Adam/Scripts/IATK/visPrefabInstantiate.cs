﻿using UnityEngine;
using System;
using IATK;

namespace Photon_IATK
{

    public class visPrefabInstantiate : MonoBehaviour
    {
        public TextAsset myDataSource;

        public delegate void OnStartAndEndLoadingVis(bool isLoading);
        public static OnStartAndEndLoadingVis visualisationLoadingDelegate;

        //this will not be used after instantiating the prefab
        private void Awake()
        {
            if (visualisationLoadingDelegate != null)
                visualisationLoadingDelegate(true);

            //label for later (not used as of now)
            this.gameObject.tag = "Vis";

            //Check if we have data
            if (myDataSource == null)
            {
                Debug.LogFormat(GlobalVariables.cError + "{0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "myDataSource is null, no visualisations will be loaded", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                throw new InvalidOperationException("Datasoucre cannot be null on Vis Prefab");
            }


            //set up datasource for the visWrapper
            CSVDataSource myCSVDataSource = createCSVDataSource(myDataSource.text);
            myCSVDataSource.data = myDataSource;

            //Add the vis wrapper to the game object this is on
            VisWrapperClass theVis = this.gameObject.AddComponent<VisWrapperClass>();

            theVis.isTriggeringEvents = false;

            theVis.wrapperCSVDataSource = myCSVDataSource;
            theVis.gameObject.name = ("ScatterplotVis_");

            theVis.visualisationType = AbstractVisualisation.VisualisationTypes.SCATTERPLOT;
            theVis.geometry = AbstractVisualisation.GeometryType.Points;
            theVis.xDimension = myCSVDataSource[0].Identifier;
            theVis.yDimension = myCSVDataSource[0].Identifier;
            theVis.zDimension = myCSVDataSource[0].Identifier;

            //theVis.updateView(AbstractVisualisation.PropertyType.GeometryType);

            theVis.CreateVisualisation(theVis.visualisationType);


            Debug.LogFormat(GlobalVariables.cOnDestory + "{0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "Destorying this script, it is no longer needed", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            if (visualisationLoadingDelegate != null)
                visualisationLoadingDelegate(false);

            theVis.gameObject.transform.localScale = new Vector3(.33f, .33f, .33f);

            theVis.isTriggeringEvents = true;

            IATK.BigMesh bigmesh;
            if (HelperFunctions.GetComponentInChild<BigMesh>(out bigmesh, theVis.gameObject, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                bigmesh.SharedMaterial.renderQueue = 2000;
            }

            Destroy(this);

        }

        CSVDataSource createCSVDataSource(string data)
        {
            CSVDataSource dataSource;
            dataSource = gameObject.AddComponent<CSVDataSource>();
            dataSource.load(data, null);
            return dataSource;
        }

    }
}