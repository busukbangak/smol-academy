using Godot;
using System;

public static class Utilities
{

    public static Godot.Collections.Dictionary MouseRaycast(Camera camera, float distance = 1000)
    {
        var mousePosition = camera.GetViewport().GetMousePosition();
        Vector3 rayStart = camera.ProjectRayOrigin(mousePosition);
        Vector3 rayEnd = rayStart + camera.ProjectRayNormal(mousePosition) * distance;
        var spaceState = camera.GetWorld().DirectSpaceState;
        return spaceState.IntersectRay(rayStart, rayEnd/* , null, 1 */);
    }
}
