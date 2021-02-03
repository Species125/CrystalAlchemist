using UnityEngine;

namespace CrystalAlchemist
{
    public class SpriteMerge : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;// assumes you've dragged a reference into this
        public Transform mergeInput;// a transform with a bunch of SpriteRenderers you want to merge

        public Sprite sprite1;
        public Sprite sprite2;
        public Sprite sprite3;

        public Texture2D debug1;
        public Texture2D debug2;
        public Texture2D debug3;

        private void Start()
        {
            this.debug1 = test(this.sprite1);
            this.debug2 = test(this.sprite2);
            this.debug3 = AddWatermark(this.debug1, this.debug2);
            this.sprite3 = Sprite.Create(this.debug3, new Rect(0.0f, 0.0f, debug3.width, debug3.height), new Vector2(0.5f, 0.5f), 100.0f);
            this.spriteRenderer.sprite = this.sprite3;
        }

        private Texture2D test(Sprite sprite)
        {
            //var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var croppedTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height, TextureFormat.RGBA32, false, false);
            croppedTexture.filterMode = FilterMode.Point;
            var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();
            return croppedTexture;
        }

        public Texture2D AddWatermark(Texture2D background, Texture2D watermark)
        {

            int startX = 0;
            int startY = background.height - watermark.height;

            for (int x = startX; x < background.width; x++)
            {

                for (int y = startY; y < background.height; y++)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    background.SetPixel(x, y, final_color);
                }
            }

            background.Apply();
            return background;
        }





        /* Takes a transform holding many sprites as input and creates one flattened sprite out of them */
        public Sprite Create(Vector2Int size, Transform input)
        {
            var spriteRenderers = input.GetComponentsInChildren<SpriteRenderer>();
            if (spriteRenderers.Length == 0)
            {
                Debug.Log("No SpriteRenderers found in " + input.name + " for SpriteMerge");
                return null;
            }

            var targetTexture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false, false);
            targetTexture.filterMode = FilterMode.Point;
            var targetPixels = targetTexture.GetPixels();
            for (int i = 0; i < targetPixels.Length; i++) targetPixels[i] = Color.clear;// default pixels are not set
            var targetWidth = targetTexture.width;

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                var sr = spriteRenderers[i];
                if (sr.sprite == null) continue;
                var position = (Vector2)sr.transform.localPosition - sr.sprite.pivot;
                var p = new Vector2Int((int)position.x, (int)position.y);
                var sourceWidth = sr.sprite.texture.width;
                // if read/write is not enabled on texture (under Advanced) then this next line throws an error
                // no way to check this without Try/Catch :(
                var sourcePixels = sr.sprite.texture.GetPixels();
                for (int j = 0; j < sourcePixels.Length; j++)
                {
                    var source = sourcePixels[j];
                    var x = (j % sourceWidth) + p.x;
                    var y = (j / sourceWidth) + p.y;
                    var index = x + y * targetWidth;
                    if (index > 0 && index < targetPixels.Length)
                    {
                        var target = targetPixels[index];
                        if (target.a > 0)
                        {
                            // alpha blend when we've already written to the target
                            float sourceAlpha = source.a;
                            float invSourceAlpha = 1f - source.a;
                            float alpha = sourceAlpha + invSourceAlpha * target.a;
                            Color result = (source * sourceAlpha + target * target.a * invSourceAlpha) / alpha;
                            result.a = alpha;
                            source = result;
                        }
                        targetPixels[index] = source;
                    }
                }
            }

            targetTexture.SetPixels(targetPixels);
            targetTexture.Apply(false, true);// read/write is disabled in 2nd param to free up memory
            return Sprite.Create(targetTexture, new Rect(new Vector2(), size), new Vector2(), 1, 0, SpriteMeshType.FullRect);
        }
    }
}
