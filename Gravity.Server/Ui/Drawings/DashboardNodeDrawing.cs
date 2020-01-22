﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gravity.Server.Configuration;
using Gravity.Server.Pipeline;
using Gravity.Server.ProcessingNodes.LoadBalancing;
using Gravity.Server.ProcessingNodes.Routing;
using Gravity.Server.ProcessingNodes.Server;
using Gravity.Server.ProcessingNodes.SpecialPurpose;
using Gravity.Server.ProcessingNodes.Transform;
using Gravity.Server.Ui.Nodes;
using Gravity.Server.Ui.Shapes;

namespace Gravity.Server.Ui.Drawings
{
    internal class DashboardNodeDrawing : NodeTile
    {
        public DashboardNodeDrawing(
            DashboardConfiguration dashboardConfiguration,
            DashboardConfiguration.NodeConfiguration nodeDrawingConfig,
            INode node)
            : base(null, nodeDrawingConfig?.Title ?? (node.Name + " Node"), "drawing", node.Disabled, 1)
        {
            LeftMargin = 20;
            RightMargin = 20;
            TopMargin = 20;
            BottomMargin = 20;

            DrawingElement nodeDrawing;

            var internalRequest = node as InternalNode;
            var response = node as ResponseNode;
            var roundRobin = node as RoundRobinNode;
            var router = node as RoutingNode;
            var server = node as ServerNode;
            var stickySession = node as StickySessionNode;
            var transform = node as TransformNode;
            var leastConnections = node as LeastConnectionsNode;
            var cors = node as CorsNode;

            if (internalRequest != null) nodeDrawing = new InternalRequestTile(this, internalRequest, nodeDrawingConfig);
            else if (response != null) nodeDrawing = new ResponseTile(this, response, nodeDrawingConfig);
            else if (roundRobin != null) nodeDrawing = new RoundRobinTile(this, roundRobin, nodeDrawingConfig, dashboardConfiguration.TrafficIndicator);
            else if (router != null) nodeDrawing = new RouterStats(this, router, nodeDrawingConfig, dashboardConfiguration.TrafficIndicator);
            else if (server != null) nodeDrawing = new ServerTile(this, server, nodeDrawingConfig);
            else if (stickySession != null) nodeDrawing = new StickySessionTile(this, stickySession, nodeDrawingConfig, dashboardConfiguration.TrafficIndicator);
            else if (transform != null) nodeDrawing = new TransformTile(this, transform, nodeDrawingConfig);
            else if (leastConnections != null) nodeDrawing = new LeastConnectionsTile(this, leastConnections, nodeDrawingConfig, dashboardConfiguration.TrafficIndicator);
            else if (cors != null) nodeDrawing = new CorsDrawing(this, cors, nodeDrawingConfig);
            else nodeDrawing = new NodeTile(this, node.Name, "", true);

            nodeDrawing.Left = 10;
            nodeDrawing.Top = 30;

            AddChild(nodeDrawing);
        }

        protected override void ArrangeChildren()
        {
            ArrangeChildrenInFixedPositions();
        }
    }
}