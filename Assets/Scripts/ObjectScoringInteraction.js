
public var currentScore: ScoringScript;


function OnCollisionEnter(collision : Collision) {    
    if (collision.gameObject.name == "Buoy"){
   
    currentScore.currentScore -= 5000;
    currentScore.Tick2();
  
  			 } 			
 } 			 

