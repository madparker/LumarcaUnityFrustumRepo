Shader "Custom/GLSL basic shader" { // defines the name of the shader 
   SubShader { // Unity chooses the subshader that fits the GPU best
      Pass { // some shaders require multiple passes
         GLSLPROGRAM // here begins the part in Unity's GLSL
         
        uniform mat4 _Object2World; // model matrix
        uniform vec4 _Time; // model matrix
         
        vec3 ro;
		vec3 rd;

		float map( in vec3 p, in vec3 center, in float radius) {
//		    // sphere: distance from the center minur radius
//		    // center of sphere is origin
//		    // basically, a map is something that returns 0 or less if ray march is in model
		    float dl = length( p - center) - radius;               // argument-position - 1
		   	dl += 0.05 * sin(10.0 * (p.z + p.y) + _Time.y * 6.0);
		    return dl;                                  // 0 or less if in model, otherwise > 0
		}

		vec3 calcNormal(in vec3 p, in vec3 center, in float radius) {
		    vec2 e = vec2(0.0001, 0.0);
		    return normalize( vec3(map(p + e.xyy, center, radius) - map(p - e.xyy, center, radius),
		                           map(p + e.yxy, center, radius) - map(p - e.yxy, center, radius),
		                           map(p + e.yyx, center, radius) - map(p - e.yyx, center, radius)));
		}

		vec4 makeSphere(in vec3 center, in vec3 color, in float radius){
			vec4 fragColor;
		
		    vec3 col = vec3(0.0);                       // color (init as black)
		    
		    float h = 1.0;
		    float t = 0.0;                              // current distance of ray march (init as 0)
		    float tmax = 20.0;							// far clipping plane
		    for (int i = 0; i < 100; i++) {
		        if (h < 0.00001 || t>tmax) break;		// stop loop when at surface or at max
		        h = map(ro + t*rd, center, radius);		// distance btwn ray endpoint and surface
		            
		        for(int x = 0; x < 7; x++){
		            for(int y = 0; y < 5; y++){
		                float sphereDist = map(ro + t*rd, vec3(x-3,y-2,0), radius);
		                
		                if(sphereDist < h){
		                	h = sphereDist;
		                }
		            }
		        }
		        
		        t += h;									// travel forward this much distance
		    }
		    
		    vec3 lig = vec3(0.5);
		    
		    if (t < tmax) {
		        vec3 pos = ro + t*rd;
		        vec3 nor = calcNormal(pos, center, radius);
		        col = vec3(1.0);
		        
		        col = vec3(0.5, 1.0, 0.5) * clamp(dot(nor, lig), 0.0, 1.0);
		    }

		    
		    fragColor = vec4(col,1.0);               // output color 
		    
		    return fragColor;
		}
 
 
         #ifdef VERTEX // here begins the vertex shader
 
         void main() // all vertex shaders define a main() function
         {
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
               // this line transforms the predefined attribute 
               // gl_Vertex of type vec4 with the predefined
               // uniform gl_ModelViewProjectionMatrix of type mat4
               // and stores the result in the predefined output 
               // variable gl_Position of type vec4.
         }
 
         #endif // here ends the definition of the vertex shader
 
 
         #ifdef FRAGMENT // here begins the fragment shader
 
         void main() // all fragment shaders define a main() function
         {
         	vec2 iResolution = vec2(500, 500);
            
		    vec3 center = vec3(1, 0, 0);
		    
			vec2 uv = gl_FragCoord.xy / iResolution.xy; // current px normalized from 0 to 1
		    vec2 p = -1.0 + 2.0*uv;                     // current px normalized from -1 to 1
		    p.x *= iResolution.x / iResolution.y;		// current px, but accounts for aspect
		    ro = vec3(0.0, 0.0, 2.0);              // ray origin (camera)
		    rd = normalize(vec3(p, -1.0));         // ray direction
   
    		gl_FragColor = makeSphere(vec3(-2, 0, 0), vec3(0, 1, 0), 0.5);
         }
 
         #endif // here ends the definition of the fragment shader
 
         ENDGLSL // here ends the part in GLSL 
      }
   }
}