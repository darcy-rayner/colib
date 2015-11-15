using UnityEngine;
using System.Collections;

namespace CoLib
{

public static partial class Commands
{
	#region Extension methods

	public static Ref<Vector3> ToPositionRef(this Transform transform, bool isLocalSpace)
	{
		Ref<Vector3> position = null;
		if (isLocalSpace) {
			position = new Ref<Vector3>( 
				() => transform.localPosition,
				newPosition => transform.localPosition = newPosition
			);
		} else {
			position = new Ref<Vector3>( 
				() => transform.position,
				newPosition => transform.position = newPosition
			);
		}
		return position;
	}

	public static Ref<Quaternion> ToRotationRef(this Transform transform, bool isLocalSpace)
	{
		Ref<Quaternion> rotation = null;
		if (isLocalSpace) {
			rotation = new Ref<Quaternion>( 
				() => transform.localRotation,
				newRotation => transform.localRotation = newRotation
			);
		} else {
			rotation = new Ref<Quaternion>( 
				() => transform.rotation,
				newRotation => transform.rotation = newRotation
			);
		}
		return rotation;
	}

	public static Ref<Vector3> ToScaleRef(this Transform transform)
	{
		Ref<Vector3> scale = new Ref<Vector3>( 
			() => transform.localScale,
			newScale => transform.localScale = newScale
		);
		return scale;
	}

	#endregion

	#region MoveTo
	
	public static CommandDelegate MoveTo(GameObject gameObject, Vector3 endPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");
		return MoveTo(gameObject.transform, endPosition, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveTo(Transform transform, Vector3 endPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeTo(transform.ToPositionRef(isLocalSpace), endPosition, duration, ease);
	}
	
	#endregion
	
	#region MoveBy

	public static CommandDelegate MoveBy(GameObject gameObject, Vector3 offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return MoveBy(gameObject.transform, offset, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveBy(Transform transform, Vector3 offset, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeBy(transform.ToPositionRef(isLocalSpace),  offset, duration, ease);
	}
	
	#endregion
	
	#region MoveFrom
	
	public static CommandDelegate MoveFrom(GameObject gameObject, Vector3 startPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return MoveFrom(gameObject.transform, startPosition, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate MoveFrom(Transform transform, Vector3 startPosition, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return ChangeFrom(transform.ToPositionRef(isLocalSpace), startPosition, duration, ease);
	}
	#endregion
	
	#region RotateTo
	
	public static CommandDelegate RotateTo(GameObject gameObject, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateTo(gameObject.transform, endRotation, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateTo(Transform transform, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateTo(transform.ToRotationRef(isLocalSpace), endRotation, duration, ease);
	}
	
	#endregion
	
	#region RotateBy
	
	public static CommandDelegate RotateBy(GameObject gameObject, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateBy(gameObject.transform, endRotation, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateBy(Transform transform, Quaternion endRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateBy(transform.ToRotationRef(isLocalSpace), endRotation, duration, ease);
	}
	
	#endregion
	
	#region RotateFrom
	
	public static CommandDelegate RotateFrom(GameObject gameObject, Quaternion startRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return RotateFrom(gameObject.transform, startRotation, duration, ease, isLocalSpace);
	}
	
	public static CommandDelegate RotateFrom(Transform transform, Quaternion startRotation, double duration, CommandEase ease = null, bool isLocalSpace = false)
	{
		CheckArgumentNonNull(transform, "transform");

		return RotateFrom(transform.ToRotationRef(isLocalSpace), startRotation, duration, ease);
	}
	
	#endregion
	
	#region ScaleTo
	
	public static CommandDelegate ScaleTo(GameObject gameObject, float endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleTo(gameObject.transform, endScale, duration, ease);
	}
	
	public static CommandDelegate ScaleTo(GameObject gameObject, Vector3 endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleTo(gameObject.transform, endScale, duration, ease);
	}
	
	public static CommandDelegate ScaleTo(Transform transform, float endScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleTo(transform, Vector3.one * endScale, duration, ease);
	}
	
	public static CommandDelegate ScaleTo(Transform transform, Vector3 endScale, double duration, CommandEase ease = null)
	{	
		CheckArgumentNonNull(transform, "transform");
	
		return ChangeTo(transform.ToScaleRef(), endScale, duration, ease);
	}
	
	#endregion
	
	#region ScaleBy
	
	public static CommandDelegate ScaleBy(GameObject gameObject, float scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleBy(gameObject.transform, scaleFactor, duration, ease);
	}
	
	public static CommandDelegate ScaleBy(GameObject gameObject, Vector3 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleBy(gameObject.transform, scaleFactor, duration, ease);
	}
	
	public static CommandDelegate ScaleBy(Transform transform, float scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleBy(transform, Vector3.one * scaleFactor, duration, ease);
	}
	
	public static CommandDelegate ScaleBy(Transform transform, Vector3 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		
		return ScaleBy(transform.ToScaleRef(), scaleFactor, duration, ease);
	}
	
	#endregion
	
	#region ScaleFrom
	
	public static CommandDelegate ScaleFrom(GameObject gameObject, float startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleFrom(gameObject.transform, startScale, duration, ease);
	}
	
	public static CommandDelegate ScaleFrom(GameObject gameObject, Vector3 startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return ScaleFrom(gameObject.transform, startScale, duration, ease);
	}
	
	public static CommandDelegate ScaleFrom(Transform transform, float startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");

		return ScaleFrom(transform, Vector3.one * startScale, duration, ease);
	}
	
	public static CommandDelegate ScaleFrom(Transform transform, Vector3 startScale, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(transform, "transform");
		
		return ChangeFrom(transform.ToScaleRef(), startScale, duration, ease);
	}
	
	#endregion
	
	#region TintTo
	
	public static CommandDelegate TintTo(GameObject gameObject, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return TintTo(gameObject.GetComponent<Renderer>().material, endColour, duration, ease);
	}
	
	public static CommandDelegate TintTo(Renderer renderer, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintTo(renderer.material, endColour, duration, ease);
	}
	
	public static CommandDelegate TintTo(Material material, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		Ref<Color> colour = new Ref<Color>( 
			() => material.color,
			newColour => material.color = newColour
		);
		return TintTo(colour, endColour, duration, ease);
	}
	
	public static CommandDelegate TintTo(SpriteRenderer renderer, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		Ref<Color> colour = new Ref<Color>( 
           () => renderer.color,
           newColour => renderer.color = newColour
        );
		return TintTo(colour, endColour, duration, ease);
	}
		
	public static CommandDelegate TintTo(GUITexture texture, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		Ref<Color> colour = new Ref<Color>( 
			() => texture.color,
			newColour => texture.color = newColour
		);
		return TintTo(colour, endColour, duration, ease);
	}

	public static CommandDelegate TintTo(GUIText text, Color endColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		Ref<Color> colour = new Ref<Color>( 
			() => text.color,
			newColour => text.color = newColour
		);
		return TintTo(colour, endColour, duration, ease);
	}
	
	#endregion
	
	#region TintBy
	
	public static CommandDelegate TintBy(GameObject gameObject, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return TintBy(gameObject.GetComponent<Renderer>().material, offset, duration, ease);
	}
	
	public static CommandDelegate TintBy(Renderer renderer, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintBy(renderer.material, offset, duration, ease);
	}
	
	public static CommandDelegate TintBy(Material material, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		Ref<Color> colour = new Ref<Color>( 
			() => material.color,
			newColour => material.color = newColour
		);
		return TintBy(colour, offset, duration, ease);
	}
	
	public static CommandDelegate TintBy(SpriteRenderer renderer, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		Ref<Color> colour = new Ref<Color>( 
           () => renderer.color,
           newColour => renderer.color = newColour
        );
		return TintBy(colour, offset, duration, ease);
	}
		
	public static CommandDelegate TintBy(GUITexture texture, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		Ref<Color> colour = new Ref<Color>( 
			() => texture.color,
			newColour => texture.color = newColour
		);
		return TintBy(colour, offset, duration, ease);
	}

	public static CommandDelegate TintBy(GUIText text, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		Ref<Color> colour = new Ref<Color>( 
			() => text.color,
			newColour => text.color = newColour
		);
		return TintBy(colour, offset, duration, ease);
	}
	
	#endregion
	
	#region TintFrom
	
	public static CommandDelegate TintFrom(GameObject gameObject, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return TintFrom(gameObject.GetComponent<Renderer>().material, startColour, duration, ease);
	}
	
	public static CommandDelegate TintFrom(Renderer renderer, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return TintFrom(renderer.material, startColour, duration, ease);
	}
	
	public static CommandDelegate TintFrom(Material material, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		Ref<Color> colour = new Ref<Color>( 
			() => material.color,
			newColour => material.color = newColour
		);
		return TintFrom(colour, startColour, duration, ease);
	}
	
	public static CommandDelegate TintFrom(SpriteRenderer renderer, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		Ref<Color> colour = new Ref<Color>( 
           () => renderer.color,
           newColour => renderer.color = newColour
        );
		return TintFrom(colour, startColour, duration, ease);
	}
		
	public static CommandDelegate TintFrom(GUITexture texture, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		Ref<Color> colour = new Ref<Color>( 
			() => texture.color,
			newColour => texture.color = newColour
		);
		return TintFrom(colour, startColour, duration, ease);
	}

	public static CommandDelegate TintFrom(GUIText text, Color startColour, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		Ref<Color> colour = new Ref<Color>( 
			() => text.color,
			newColour => text.color = newColour
		);
		return TintFrom(colour, startColour, duration, ease);
	}
	
	#endregion
		
	#region AlphaTo
	
	public static CommandDelegate AlphaTo(GameObject gameObject, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaTo(gameObject.GetComponent<Renderer>().material, endAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaTo(Renderer renderer, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaTo(renderer.material, endAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaTo(Material material, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		Ref<Color> colour = new Ref<Color>( 
			() => material.color,
			newColour => material.color = newColour
		);
		return AlphaTo(colour, endAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaTo(SpriteRenderer renderer, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		Ref<Color> colour = new Ref<Color>( 
           () => renderer.color,
           newColour => renderer.color = newColour
          );
		return AlphaTo(colour, endAlpha, duration, ease);
	}
		
	public static CommandDelegate AlphaTo(GUITexture texture, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		Ref<Color> colour = new Ref<Color>( 
			() => texture.color,
			newColour => texture.color = newColour
		);
		return AlphaTo(colour, endAlpha, duration, ease);
	}

	public static CommandDelegate AlphaTo(GUIText text, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		Ref<Color> colour = new Ref<Color>( 
			() => text.color,
			newColour => text.color = newColour
		);
		return AlphaTo(colour, endAlpha, duration, ease);
	}
	
	#endregion
		
	#region AlphaBy
	
	public static CommandDelegate AlphaBy(GameObject gameObject, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaBy(gameObject.GetComponent<Renderer>().material, offset, duration, ease);
	}
	
	public static CommandDelegate AlphaBy(Renderer renderer, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaBy(renderer.material, offset, duration, ease);
	}
	
	public static CommandDelegate AlphaBy(Material material, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		Ref<Color> colour = new Ref<Color>( 
			() => material.color,
			newColour => material.color = newColour
		);
		return AlphaBy(colour, offset, duration, ease);
	}
	public static CommandDelegate AlphaBy(SpriteRenderer renderer, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		Ref<Color> colour = new Ref<Color>( 
           () => renderer.color,
           newColour => renderer.color = newColour
        );
		return AlphaBy(colour, offset, duration, ease);
	}
		
	public static CommandDelegate AlphaBy(GUITexture texture, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		Ref<Color> colour = new Ref<Color>( 
			() => texture.color,
			newColour => texture.color = newColour
		);
		return AlphaBy(colour, offset, duration, ease);
	}

	public static CommandDelegate AlphaBy(GUIText text, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		Ref<Color> colour = new Ref<Color>( 
			() => text.color,
			newColour => text.color = newColour
		);
		return AlphaBy(colour, offset, duration, ease);
	}
	
	#endregion

	#region AlphaFrom
	
	public static CommandDelegate AlphaFrom(GameObject gameObject, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(gameObject, "gameObject");

		return AlphaFrom(gameObject.GetComponent<Renderer>().material, startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(Renderer renderer, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		return AlphaFrom(renderer.material, startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(Material material, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(material, "material");

		Ref<Color> colour = new Ref<Color>( 
			() => material.color,
			newColour => material.color = newColour
		);
		return AlphaFrom(colour, startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(SpriteRenderer renderer, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(renderer, "renderer");

		Ref<Color> colour = new Ref<Color>( 
           () => renderer.color,
           newColour => renderer.color = newColour
        );
		return AlphaFrom(colour, startAlpha, duration, ease);
	}
		
	public static CommandDelegate AlphaFrom(GUITexture texture, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(texture, "texture");

		Ref<Color> colour = new Ref<Color>( 
			() => texture.color,
			newColour => texture.color = newColour
		);
		return AlphaFrom(colour, startAlpha, duration, ease);
	}
	
	public static CommandDelegate AlphaFrom(GUIText text, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(text, "text");

		Ref<Color> colour = new Ref<Color>( 
			() => text.color,
			newColour => text.color = newColour
		);
		return AlphaFrom(colour, startAlpha, duration, ease);
	}

	#endregion
}
	
}