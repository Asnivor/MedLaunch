using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class MednafenControls
    {
        // gb
        public string gb__input__builtin__gamepad__a { get; set; }                              // gb, Built-In, Gamepad: A
        public string gb__input__builtin__gamepad__b { get; set; }                              // gb, Built-In, Gamepad: B
        public string gb__input__builtin__gamepad__down { get; set; }                           // gb, Built-In, Gamepad: DOWN ↓
        public string gb__input__builtin__gamepad__left { get; set; }                           // gb, Built-In, Gamepad: LEFT ←
        public string gb__input__builtin__gamepad__rapid_a { get; set; }                        // gb, Built-In, Gamepad: Rapid A
        public string gb__input__builtin__gamepad__rapid_b { get; set; }                        // gb, Built-In, Gamepad: Rapid B
        public string gb__input__builtin__gamepad__right { get; set; }                          // gb, Built-In, Gamepad: RIGHT →
        public string gb__input__builtin__gamepad__select { get; set; }                         // gb, Built-In, Gamepad: SELECT
        public string gb__input__builtin__gamepad__start { get; set; }                          // gb, Built-In, Gamepad: START
        public string gb__input__builtin__gamepad__up { get; set; }                             // gb, Built-In, Gamepad: UP ↑
        public string gb__input__tilt__tilt__down { get; set; }                                 // gb, Tilt, Tilt: DOWN ↓
        public string gb__input__tilt__tilt__left { get; set; }                                 // gb, Tilt, Tilt: LEFT ←
        public string gb__input__tilt__tilt__right { get; set; }                                // gb, Tilt, Tilt: RIGHT →
        public string gb__input__tilt__tilt__up { get; set; }                                   // gb, Tilt, Tilt: UP ↑

        // gba
        public string gba__input__builtin__gamepad__a { get; set; }                              // gba, Built-In, Gamepad: A
        public string gba__input__builtin__gamepad__b { get; set; }                              // gba, Built-In, Gamepad: B
        public string gba__input__builtin__gamepad__down { get; set; }                           // gba, Built-In, Gamepad: DOWN ↓
        public string gba__input__builtin__gamepad__left { get; set; }                           // gba, Built-In, Gamepad: LEFT ←
        public string gba__input__builtin__gamepad__rapid_a { get; set; }                        // gba, Built-In, Gamepad: Rapid A
        public string gba__input__builtin__gamepad__rapid_b { get; set; }                        // gba, Built-In, Gamepad: Rapid B
        public string gba__input__builtin__gamepad__right { get; set; }                          // gba, Built-In, Gamepad: RIGHT →
        public string gba__input__builtin__gamepad__select { get; set; }                         // gba, Built-In, Gamepad: SELECT
        public string gba__input__builtin__gamepad__shoulder_l { get; set; }                     // gba, Built-In, Gamepad: SHOULDER L
        public string gba__input__builtin__gamepad__shoulder_r { get; set; }                     // gba, Built-In, Gamepad: SHOULDER R
        public string gba__input__builtin__gamepad__start { get; set; }                          // gba, Built-In, Gamepad: START
        public string gba__input__builtin__gamepad__up { get; set; }                             // gba, Built-In, Gamepad: UP ↑

        // gg
        public string gg__input__builtin__gamepad__button1 { get; set; }                        // gg, Built-In, Gamepad: Button 1
        public string gg__input__builtin__gamepad__button2 { get; set; }                        // gg, Built-In, Gamepad: Button 2
        public string gg__input__builtin__gamepad__down { get; set; }                           // gg, Built-In, Gamepad: DOWN ↓
        public string gg__input__builtin__gamepad__left { get; set; }                           // gg, Built-In, Gamepad: LEFT ←
        public string gg__input__builtin__gamepad__rapid_button1 { get; set; }                  // gg, Built-In, Gamepad: Rapid Button 1
        public string gg__input__builtin__gamepad__rapid_button2 { get; set; }                  // gg, Built-In, Gamepad: Rapid Button 2
        public string gg__input__builtin__gamepad__right { get; set; }                          // gg, Built-In, Gamepad: RIGHT →
        public string gg__input__builtin__gamepad__start { get; set; }                          // gg, Built-In, Gamepad: START
        public string gg__input__builtin__gamepad__up { get; set; }                             // gg, Built-In, Gamepad: UP ↑

        // lynx
        public string lynx__input__builtin__gamepad__a { get; set; }                            // lynx, Built-In, Gamepad: A (outer)
        public string lynx__input__builtin__gamepad__b { get; set; }                            // lynx, Built-In, Gamepad: B (inner)
        public string lynx__input__builtin__gamepad__down { get; set; }                         // lynx, Built-In, Gamepad: DOWN ↓
        public string lynx__input__builtin__gamepad__left { get; set; }                         // lynx, Built-In, Gamepad: LEFT ←
        public string lynx__input__builtin__gamepad__option_1 { get; set; }                     // lynx, Built-In, Gamepad: Option 1 (upper)
        public string lynx__input__builtin__gamepad__option_2 { get; set; }                     // lynx, Built-In, Gamepad: Option 2 (lower)
        public string lynx__input__builtin__gamepad__pause { get; set; }                        // lynx, Built-In, Gamepad: PAUSE
        public string lynx__input__builtin__gamepad__rapid_a { get; set; }                      // lynx, Built-In, Gamepad: Rapid A (outer)
        public string lynx__input__builtin__gamepad__rapid_b { get; set; }                      // lynx, Built-In, Gamepad: Rapid B (inner)
        public string lynx__input__builtin__gamepad__rapid_option_1 { get; set; }               // lynx, Built-In, Gamepad: Rapid Option 1 (upper)
        public string lynx__input__builtin__gamepad__rapid_option_2 { get; set; }               // lynx, Built-In, Gamepad: Rapid Option 2 (lower)
        public string lynx__input__builtin__gamepad__right { get; set; }                        // lynx, Built-In, Gamepad: RIGHT →
        public string lynx__input__builtin__gamepad__up { get; set; }                           // lynx, Built-In, Gamepad: UP ↑

        // md

        /* 3-button */
        public string md__input__port1__gamepad__a { get; set; }                                // md, Virtual Port 1, 3-Button Gamepad: A         
        public string md__input__port1__gamepad__b { get; set; }                                // md, Virtual Port 1, 3-Button Gamepad: B   
        public string md__input__port1__gamepad__c { get; set; }                                // md, Virtual Port 1, 3-Button Gamepad: C   
        public string md__input__port1__gamepad__down { get; set; }                             // md, Virtual Port 1, 3-Button Gamepad: DOWN ↓
        public string md__input__port1__gamepad__left { get; set; }                             // md, Virtual Port 1, 3-Button Gamepad: LEFT ←
        public string md__input__port1__gamepad__rapid_a { get; set; }                          // md, Virtual Port 1, 3-Button Gamepad: Rapid A  
        public string md__input__port1__gamepad__rapid_b { get; set; }                          // md, Virtual Port 1, 3-Button Gamepad: Rapid B  
        public string md__input__port1__gamepad__rapid_c { get; set; }                          // md, Virtual Port 1, 3-Button Gamepad: Rapid C 
        public string md__input__port1__gamepad__right { get; set; }                            // md, Virtual Port 1, 3-Button Gamepad: RIGHT →
        public string md__input__port1__gamepad__start { get; set; }                            // md, Virtual Port 1, 3-Button Gamepad: Start
        public string md__input__port1__gamepad__up { get; set; }                               // md, Virtual Port 1, 3-Button Gamepad: UP ↑

        public string md__input__port2__gamepad__a { get; set; }                                // md, Virtual Port 2, 3-Button Gamepad: A         
        public string md__input__port2__gamepad__b { get; set; }                                // md, Virtual Port 2, 3-Button Gamepad: B   
        public string md__input__port2__gamepad__c { get; set; }                                // md, Virtual Port 2, 3-Button Gamepad: C   
        public string md__input__port2__gamepad__down { get; set; }                             // md, Virtual Port 2, 3-Button Gamepad: DOWN ↓
        public string md__input__port2__gamepad__left { get; set; }                             // md, Virtual Port 2, 3-Button Gamepad: LEFT ←
        public string md__input__port2__gamepad__rapid_a { get; set; }                          // md, Virtual Port 2, 3-Button Gamepad: Rapid A  
        public string md__input__port2__gamepad__rapid_b { get; set; }                          // md, Virtual Port 2, 3-Button Gamepad: Rapid B  
        public string md__input__port2__gamepad__rapid_c { get; set; }                          // md, Virtual Port 2, 3-Button Gamepad: Rapid C 
        public string md__input__port2__gamepad__right { get; set; }                            // md, Virtual Port 2, 3-Button Gamepad: RIGHT →
        public string md__input__port2__gamepad__start { get; set; }                            // md, Virtual Port 2, 3-Button Gamepad: Start
        public string md__input__port2__gamepad__up { get; set; }                               // md, Virtual Port 2, 3-Button Gamepad: UP ↑

        public string md__input__port3__gamepad__a { get; set; }                                // md, Virtual Port 3, 3-Button Gamepad: A         
        public string md__input__port3__gamepad__b { get; set; }                                // md, Virtual Port 3, 3-Button Gamepad: B   
        public string md__input__port3__gamepad__c { get; set; }                                // md, Virtual Port 3, 3-Button Gamepad: C   
        public string md__input__port3__gamepad__down { get; set; }                             // md, Virtual Port 3, 3-Button Gamepad: DOWN ↓
        public string md__input__port3__gamepad__left { get; set; }                             // md, Virtual Port 3, 3-Button Gamepad: LEFT ←
        public string md__input__port3__gamepad__rapid_a { get; set; }                          // md, Virtual Port 3, 3-Button Gamepad: Rapid A  
        public string md__input__port3__gamepad__rapid_b { get; set; }                          // md, Virtual Port 3, 3-Button Gamepad: Rapid B  
        public string md__input__port3__gamepad__rapid_c { get; set; }                          // md, Virtual Port 3, 3-Button Gamepad: Rapid C 
        public string md__input__port3__gamepad__right { get; set; }                            // md, Virtual Port 3, 3-Button Gamepad: RIGHT →
        public string md__input__port3__gamepad__start { get; set; }                            // md, Virtual Port 3, 3-Button Gamepad: Start
        public string md__input__port3__gamepad__up { get; set; }                               // md, Virtual Port 3, 3-Button Gamepad: UP ↑

        public string md__input__port4__gamepad__a { get; set; }                                // md, Virtual Port 4, 3-Button Gamepad: A         
        public string md__input__port4__gamepad__b { get; set; }                                // md, Virtual Port 4, 3-Button Gamepad: B   
        public string md__input__port4__gamepad__c { get; set; }                                // md, Virtual Port 4, 3-Button Gamepad: C   
        public string md__input__port4__gamepad__down { get; set; }                             // md, Virtual Port 4, 3-Button Gamepad: DOWN ↓
        public string md__input__port4__gamepad__left { get; set; }                             // md, Virtual Port 4, 3-Button Gamepad: LEFT ←
        public string md__input__port4__gamepad__rapid_a { get; set; }                          // md, Virtual Port 4, 3-Button Gamepad: Rapid A  
        public string md__input__port4__gamepad__rapid_b { get; set; }                          // md, Virtual Port 4, 3-Button Gamepad: Rapid B  
        public string md__input__port4__gamepad__rapid_c { get; set; }                          // md, Virtual Port 4, 3-Button Gamepad: Rapid C 
        public string md__input__port4__gamepad__right { get; set; }                            // md, Virtual Port 4, 3-Button Gamepad: RIGHT →
        public string md__input__port4__gamepad__start { get; set; }                            // md, Virtual Port 4, 3-Button Gamepad: Start
        public string md__input__port4__gamepad__up { get; set; }                               // md, Virtual Port 4, 3-Button Gamepad: UP ↑

        public string md__input__port5__gamepad__a { get; set; }                                // md, Virtual Port 5, 3-Button Gamepad: A         
        public string md__input__port5__gamepad__b { get; set; }                                // md, Virtual Port 5, 3-Button Gamepad: B   
        public string md__input__port5__gamepad__c { get; set; }                                // md, Virtual Port 5, 3-Button Gamepad: C   
        public string md__input__port5__gamepad__down { get; set; }                             // md, Virtual Port 5, 3-Button Gamepad: DOWN ↓
        public string md__input__port5__gamepad__left { get; set; }                             // md, Virtual Port 5, 3-Button Gamepad: LEFT ←
        public string md__input__port5__gamepad__rapid_a { get; set; }                          // md, Virtual Port 5, 3-Button Gamepad: Rapid A  
        public string md__input__port5__gamepad__rapid_b { get; set; }                          // md, Virtual Port 5, 3-Button Gamepad: Rapid B  
        public string md__input__port5__gamepad__rapid_c { get; set; }                          // md, Virtual Port 5, 3-Button Gamepad: Rapid C 
        public string md__input__port5__gamepad__right { get; set; }                            // md, Virtual Port 5, 3-Button Gamepad: RIGHT →
        public string md__input__port5__gamepad__start { get; set; }                            // md, Virtual Port 5, 3-Button Gamepad: Start
        public string md__input__port5__gamepad__up { get; set; }                               // md, Virtual Port 5, 3-Button Gamepad: UP ↑

        public string md__input__port6__gamepad__a { get; set; }                                // md, Virtual Port 6, 3-Button Gamepad: A         
        public string md__input__port6__gamepad__b { get; set; }                                // md, Virtual Port 6, 3-Button Gamepad: B   
        public string md__input__port6__gamepad__c { get; set; }                                // md, Virtual Port 6, 3-Button Gamepad: C   
        public string md__input__port6__gamepad__down { get; set; }                             // md, Virtual Port 6, 3-Button Gamepad: DOWN ↓
        public string md__input__port6__gamepad__left { get; set; }                             // md, Virtual Port 6, 3-Button Gamepad: LEFT ←
        public string md__input__port6__gamepad__rapid_a { get; set; }                          // md, Virtual Port 6, 3-Button Gamepad: Rapid A  
        public string md__input__port6__gamepad__rapid_b { get; set; }                          // md, Virtual Port 6, 3-Button Gamepad: Rapid B  
        public string md__input__port6__gamepad__rapid_c { get; set; }                          // md, Virtual Port 6, 3-Button Gamepad: Rapid C 
        public string md__input__port6__gamepad__right { get; set; }                            // md, Virtual Port 6, 3-Button Gamepad: RIGHT →
        public string md__input__port6__gamepad__start { get; set; }                            // md, Virtual Port 6, 3-Button Gamepad: Start
        public string md__input__port6__gamepad__up { get; set; }                               // md, Virtual Port 6, 3-Button Gamepad: UP ↑

        public string md__input__port7__gamepad__a { get; set; }                                // md, Virtual Port 7, 3-Button Gamepad: A         
        public string md__input__port7__gamepad__b { get; set; }                                // md, Virtual Port 7, 3-Button Gamepad: B   
        public string md__input__port7__gamepad__c { get; set; }                                // md, Virtual Port 7, 3-Button Gamepad: C   
        public string md__input__port7__gamepad__down { get; set; }                             // md, Virtual Port 7, 3-Button Gamepad: DOWN ↓
        public string md__input__port7__gamepad__left { get; set; }                             // md, Virtual Port 7, 3-Button Gamepad: LEFT ←
        public string md__input__port7__gamepad__rapid_a { get; set; }                          // md, Virtual Port 7, 3-Button Gamepad: Rapid A  
        public string md__input__port7__gamepad__rapid_b { get; set; }                          // md, Virtual Port 7, 3-Button Gamepad: Rapid B  
        public string md__input__port7__gamepad__rapid_c { get; set; }                          // md, Virtual Port 7, 3-Button Gamepad: Rapid C 
        public string md__input__port7__gamepad__right { get; set; }                            // md, Virtual Port 7, 3-Button Gamepad: RIGHT →
        public string md__input__port7__gamepad__start { get; set; }                            // md, Virtual Port 7, 3-Button Gamepad: Start
        public string md__input__port7__gamepad__up { get; set; }                               // md, Virtual Port 7, 3-Button Gamepad: UP ↑

        public string md__input__port8__gamepad__a { get; set; }                                // md, Virtual Port 8, 3-Button Gamepad: A         
        public string md__input__port8__gamepad__b { get; set; }                                // md, Virtual Port 8, 3-Button Gamepad: B   
        public string md__input__port8__gamepad__c { get; set; }                                // md, Virtual Port 8, 3-Button Gamepad: C   
        public string md__input__port8__gamepad__down { get; set; }                             // md, Virtual Port 8, 3-Button Gamepad: DOWN ↓
        public string md__input__port8__gamepad__left { get; set; }                             // md, Virtual Port 8, 3-Button Gamepad: LEFT ←
        public string md__input__port8__gamepad__rapid_a { get; set; }                          // md, Virtual Port 8, 3-Button Gamepad: Rapid A  
        public string md__input__port8__gamepad__rapid_b { get; set; }                          // md, Virtual Port 8, 3-Button Gamepad: Rapid B  
        public string md__input__port8__gamepad__rapid_c { get; set; }                          // md, Virtual Port 8, 3-Button Gamepad: Rapid C 
        public string md__input__port8__gamepad__right { get; set; }                            // md, Virtual Port 8, 3-Button Gamepad: RIGHT →
        public string md__input__port8__gamepad__start { get; set; }                            // md, Virtual Port 8, 3-Button Gamepad: Start
        public string md__input__port8__gamepad__up { get; set; }                               // md, Virtual Port 8, 3-Button Gamepad: UP ↑

        /* 2-Button */
        public string md__input__port1__gamepad1__a { get; set; }                                // md, Virtual Port 1, 1-Button Gamepad: A         
        public string md__input__port1__gamepad1__b { get; set; }                                // md, Virtual Port 1, 1-Button Gamepad: B    
        public string md__input__port1__gamepad1__down { get; set; }                             // md, Virtual Port 1, 1-Button Gamepad: DOWN ↓
        public string md__input__port1__gamepad1__left { get; set; }                             // md, Virtual Port 1, 1-Button Gamepad: LEFT ←
        public string md__input__port1__gamepad1__rapid_a { get; set; }                          // md, Virtual Port 1, 1-Button Gamepad: Rapid A  
        public string md__input__port1__gamepad1__rapid_b { get; set; }                          // md, Virtual Port 1, 1-Button Gamepad: Rapid B 
        public string md__input__port1__gamepad1__right { get; set; }                            // md, Virtual Port 1, 1-Button Gamepad: RIGHT →
        public string md__input__port1__gamepad1__start { get; set; }                            // md, Virtual Port 1, 1-Button Gamepad: Start
        public string md__input__port1__gamepad1__up { get; set; }                               // md, Virtual Port 1, 1-Button Gamepad: UP ↑

        public string md__input__port2__gamepad2__a { get; set; }                                // md, Virtual Port 2, 2-Button Gamepad: A         
        public string md__input__port2__gamepad2__b { get; set; }                                // md, Virtual Port 2, 2-Button Gamepad: B    
        public string md__input__port2__gamepad2__down { get; set; }                             // md, Virtual Port 2, 2-Button Gamepad: DOWN ↓
        public string md__input__port2__gamepad2__left { get; set; }                             // md, Virtual Port 2, 2-Button Gamepad: LEFT ←
        public string md__input__port2__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 2, 2-Button Gamepad: Rapid A  
        public string md__input__port2__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 2, 2-Button Gamepad: Rapid B 
        public string md__input__port2__gamepad2__right { get; set; }                            // md, Virtual Port 2, 2-Button Gamepad: RIGHT →
        public string md__input__port2__gamepad2__start { get; set; }                            // md, Virtual Port 2, 2-Button Gamepad: Start
        public string md__input__port2__gamepad2__up { get; set; }                               // md, Virtual Port 2, 2-Button Gamepad: UP ↑

        public string md__input__port3__gamepad2__a { get; set; }                                // md, Virtual Port 3, 2-Button Gamepad: A         
        public string md__input__port3__gamepad2__b { get; set; }                                // md, Virtual Port 3, 2-Button Gamepad: B    
        public string md__input__port3__gamepad2__down { get; set; }                             // md, Virtual Port 3, 2-Button Gamepad: DOWN ↓
        public string md__input__port3__gamepad2__left { get; set; }                             // md, Virtual Port 3, 2-Button Gamepad: LEFT ←
        public string md__input__port3__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 3, 2-Button Gamepad: Rapid A  
        public string md__input__port3__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 3, 2-Button Gamepad: Rapid B 
        public string md__input__port3__gamepad2__right { get; set; }                            // md, Virtual Port 3, 2-Button Gamepad: RIGHT →
        public string md__input__port3__gamepad2__start { get; set; }                            // md, Virtual Port 3, 2-Button Gamepad: Start
        public string md__input__port3__gamepad2__up { get; set; }                               // md, Virtual Port 3, 2-Button Gamepad: UP ↑

        public string md__input__port4__gamepad2__a { get; set; }                                // md, Virtual Port 4, 2-Button Gamepad: A         
        public string md__input__port4__gamepad2__b { get; set; }                                // md, Virtual Port 4, 2-Button Gamepad: B    
        public string md__input__port4__gamepad2__down { get; set; }                             // md, Virtual Port 4, 2-Button Gamepad: DOWN ↓
        public string md__input__port4__gamepad2__left { get; set; }                             // md, Virtual Port 4, 2-Button Gamepad: LEFT ←
        public string md__input__port4__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 4, 2-Button Gamepad: Rapid A  
        public string md__input__port4__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 4, 2-Button Gamepad: Rapid B 
        public string md__input__port4__gamepad2__right { get; set; }                            // md, Virtual Port 4, 2-Button Gamepad: RIGHT →
        public string md__input__port4__gamepad2__start { get; set; }                            // md, Virtual Port 4, 2-Button Gamepad: Start
        public string md__input__port4__gamepad2__up { get; set; }                               // md, Virtual Port 4, 2-Button Gamepad: UP ↑

        public string md__input__port5__gamepad2__a { get; set; }                                // md, Virtual Port 5, 2-Button Gamepad: A         
        public string md__input__port5__gamepad2__b { get; set; }                                // md, Virtual Port 5, 2-Button Gamepad: B    
        public string md__input__port5__gamepad2__down { get; set; }                             // md, Virtual Port 5, 2-Button Gamepad: DOWN ↓
        public string md__input__port5__gamepad2__left { get; set; }                             // md, Virtual Port 5, 2-Button Gamepad: LEFT ←
        public string md__input__port5__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 5, 2-Button Gamepad: Rapid A  
        public string md__input__port5__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 5, 2-Button Gamepad: Rapid B 
        public string md__input__port5__gamepad2__right { get; set; }                            // md, Virtual Port 5, 2-Button Gamepad: RIGHT →
        public string md__input__port5__gamepad2__start { get; set; }                            // md, Virtual Port 5, 2-Button Gamepad: Start
        public string md__input__port5__gamepad2__up { get; set; }                               // md, Virtual Port 5, 2-Button Gamepad: UP ↑

        public string md__input__port6__gamepad2__a { get; set; }                                // md, Virtual Port 6, 2-Button Gamepad: A         
        public string md__input__port6__gamepad2__b { get; set; }                                // md, Virtual Port 6, 2-Button Gamepad: B    
        public string md__input__port6__gamepad2__down { get; set; }                             // md, Virtual Port 6, 2-Button Gamepad: DOWN ↓
        public string md__input__port6__gamepad2__left { get; set; }                             // md, Virtual Port 6, 2-Button Gamepad: LEFT ←
        public string md__input__port6__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 6, 2-Button Gamepad: Rapid A  
        public string md__input__port6__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 6, 2-Button Gamepad: Rapid B 
        public string md__input__port6__gamepad2__right { get; set; }                            // md, Virtual Port 6, 2-Button Gamepad: RIGHT →
        public string md__input__port6__gamepad2__start { get; set; }                            // md, Virtual Port 6, 2-Button Gamepad: Start
        public string md__input__port6__gamepad2__up { get; set; }                               // md, Virtual Port 6, 2-Button Gamepad: UP ↑

        public string md__input__port7__gamepad2__a { get; set; }                                // md, Virtual Port 7, 2-Button Gamepad: A         
        public string md__input__port7__gamepad2__b { get; set; }                                // md, Virtual Port 7, 2-Button Gamepad: B    
        public string md__input__port7__gamepad2__down { get; set; }                             // md, Virtual Port 7, 2-Button Gamepad: DOWN ↓
        public string md__input__port7__gamepad2__left { get; set; }                             // md, Virtual Port 7, 2-Button Gamepad: LEFT ←
        public string md__input__port7__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 7, 2-Button Gamepad: Rapid A  
        public string md__input__port7__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 7, 2-Button Gamepad: Rapid B 
        public string md__input__port7__gamepad2__right { get; set; }                            // md, Virtual Port 7, 2-Button Gamepad: RIGHT →
        public string md__input__port7__gamepad2__start { get; set; }                            // md, Virtual Port 7, 2-Button Gamepad: Start
        public string md__input__port7__gamepad2__up { get; set; }                               // md, Virtual Port 7, 2-Button Gamepad: UP ↑

        public string md__input__port8__gamepad2__a { get; set; }                                // md, Virtual Port 8, 2-Button Gamepad: A         
        public string md__input__port8__gamepad2__b { get; set; }                                // md, Virtual Port 8, 2-Button Gamepad: B    
        public string md__input__port8__gamepad2__down { get; set; }                             // md, Virtual Port 8, 2-Button Gamepad: DOWN ↓
        public string md__input__port8__gamepad2__left { get; set; }                             // md, Virtual Port 8, 2-Button Gamepad: LEFT ←
        public string md__input__port8__gamepad2__rapid_a { get; set; }                          // md, Virtual Port 8, 2-Button Gamepad: Rapid A  
        public string md__input__port8__gamepad2__rapid_b { get; set; }                          // md, Virtual Port 8, 2-Button Gamepad: Rapid B 
        public string md__input__port8__gamepad2__right { get; set; }                            // md, Virtual Port 8, 2-Button Gamepad: RIGHT →
        public string md__input__port8__gamepad2__start { get; set; }                            // md, Virtual Port 8, 2-Button Gamepad: Start
        public string md__input__port8__gamepad2__up { get; set; }                               // md, Virtual Port 8, 2-Button Gamepad: UP ↑

        /* 6-Button */
        public string md__input__port1__gamepad6__a { get; set; }                                // md, Virtual Port 1, 6-button Gamepad: A         
        public string md__input__port1__gamepad6__b { get; set; }                                // md, Virtual Port 1, 6-button Gamepad: B   
        public string md__input__port1__gamepad6__c { get; set; }                                // md, Virtual Port 1, 6-button Gamepad: C   
        public string md__input__port1__gamepad6__down { get; set; }                             // md, Virtual Port 1, 6-button Gamepad: DOWN ↓
        public string md__input__port1__gamepad6__left { get; set; }                             // md, Virtual Port 1, 6-button Gamepad: LEFT ←
        public string md__input__port1__gamepad6__mode { get; set; }                             // md, Virtual Port 1, 6-Button Gamepad: Mode
        public string md__input__port1__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 1, 6-button Gamepad: Rapid A  
        public string md__input__port1__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 1, 6-button Gamepad: Rapid B  
        public string md__input__port1__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 1, 6-button Gamepad: Rapid C 
        public string md__input__port1__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 1, 6-button Gamepad: Rapid X  
        public string md__input__port1__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 1, 6-button Gamepad: Rapid Y  
        public string md__input__port1__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 1, 6-button Gamepad: Rapid Z 
        public string md__input__port1__gamepad6__right { get; set; }                            // md, Virtual Port 1, 6-button Gamepad: RIGHT →
        public string md__input__port1__gamepad6__start { get; set; }                            // md, Virtual Port 1, 6-button Gamepad: Start
        public string md__input__port1__gamepad6__up { get; set; }                               // md, Virtual Port 1, 6-button Gamepad: UP ↑
        public string md__input__port1__gamepad6__x { get; set; }                                // md, Virtual Port 1, 6-button Gamepad: X        
        public string md__input__port1__gamepad6__y { get; set; }                                // md, Virtual Port 1, 6-button Gamepad: Y   
        public string md__input__port1__gamepad6__z { get; set; }                                // md, Virtual Port 1, 6-button Gamepad: Z  

        public string md__input__port2__gamepad6__a { get; set; }                                // md, Virtual Port 2, 6-button Gamepad: A         
        public string md__input__port2__gamepad6__b { get; set; }                                // md, Virtual Port 2, 6-button Gamepad: B   
        public string md__input__port2__gamepad6__c { get; set; }                                // md, Virtual Port 2, 6-button Gamepad: C   
        public string md__input__port2__gamepad6__down { get; set; }                             // md, Virtual Port 2, 6-button Gamepad: DOWN ↓
        public string md__input__port2__gamepad6__left { get; set; }                             // md, Virtual Port 2, 6-button Gamepad: LEFT ←
        public string md__input__port2__gamepad6__mode { get; set; }                             // md, Virtual Port 2, 6-Button Gamepad: Mode
        public string md__input__port2__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 2, 6-button Gamepad: Rapid A  
        public string md__input__port2__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 2, 6-button Gamepad: Rapid B  
        public string md__input__port2__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 2, 6-button Gamepad: Rapid C 
        public string md__input__port2__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 2, 6-button Gamepad: Rapid X  
        public string md__input__port2__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 2, 6-button Gamepad: Rapid Y  
        public string md__input__port2__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 2, 6-button Gamepad: Rapid Z 
        public string md__input__port2__gamepad6__right { get; set; }                            // md, Virtual Port 2, 6-button Gamepad: RIGHT →
        public string md__input__port2__gamepad6__start { get; set; }                            // md, Virtual Port 2, 6-button Gamepad: Start
        public string md__input__port2__gamepad6__up { get; set; }                               // md, Virtual Port 2, 6-button Gamepad: UP ↑
        public string md__input__port2__gamepad6__x { get; set; }                                // md, Virtual Port 2, 6-button Gamepad: X        
        public string md__input__port2__gamepad6__y { get; set; }                                // md, Virtual Port 2, 6-button Gamepad: Y   
        public string md__input__port2__gamepad6__z { get; set; }                                // md, Virtual Port 2, 6-button Gamepad: Z  

        public string md__input__port3__gamepad6__a { get; set; }                                // md, Virtual Port 3, 6-button Gamepad: A         
        public string md__input__port3__gamepad6__b { get; set; }                                // md, Virtual Port 3, 6-button Gamepad: B   
        public string md__input__port3__gamepad6__c { get; set; }                                // md, Virtual Port 3, 6-button Gamepad: C   
        public string md__input__port3__gamepad6__down { get; set; }                             // md, Virtual Port 3, 6-button Gamepad: DOWN ↓
        public string md__input__port3__gamepad6__left { get; set; }                             // md, Virtual Port 3, 6-button Gamepad: LEFT ←
        public string md__input__port3__gamepad6__mode { get; set; }                             // md, Virtual Port 3, 6-Button Gamepad: Mode
        public string md__input__port3__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 3, 6-button Gamepad: Rapid A  
        public string md__input__port3__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 3, 6-button Gamepad: Rapid B  
        public string md__input__port3__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 3, 6-button Gamepad: Rapid C 
        public string md__input__port3__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 3, 6-button Gamepad: Rapid X  
        public string md__input__port3__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 3, 6-button Gamepad: Rapid Y  
        public string md__input__port3__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 3, 6-button Gamepad: Rapid Z 
        public string md__input__port3__gamepad6__right { get; set; }                            // md, Virtual Port 3, 6-button Gamepad: RIGHT →
        public string md__input__port3__gamepad6__start { get; set; }                            // md, Virtual Port 3, 6-button Gamepad: Start
        public string md__input__port3__gamepad6__up { get; set; }                               // md, Virtual Port 3, 6-button Gamepad: UP ↑
        public string md__input__port3__gamepad6__x { get; set; }                                // md, Virtual Port 3, 6-button Gamepad: X        
        public string md__input__port3__gamepad6__y { get; set; }                                // md, Virtual Port 3, 6-button Gamepad: Y   
        public string md__input__port3__gamepad6__z { get; set; }                                // md, Virtual Port 3, 6-button Gamepad: Z  

        public string md__input__port4__gamepad6__a { get; set; }                                // md, Virtual Port 4, 6-button Gamepad: A         
        public string md__input__port4__gamepad6__b { get; set; }                                // md, Virtual Port 4, 6-button Gamepad: B   
        public string md__input__port4__gamepad6__c { get; set; }                                // md, Virtual Port 4, 6-button Gamepad: C   
        public string md__input__port4__gamepad6__down { get; set; }                             // md, Virtual Port 4, 6-button Gamepad: DOWN ↓
        public string md__input__port4__gamepad6__left { get; set; }                             // md, Virtual Port 4, 6-button Gamepad: LEFT ←
        public string md__input__port4__gamepad6__mode { get; set; }                             // md, Virtual Port 4, 6-Button Gamepad: Mode
        public string md__input__port4__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 4, 6-button Gamepad: Rapid A  
        public string md__input__port4__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 4, 6-button Gamepad: Rapid B  
        public string md__input__port4__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 4, 6-button Gamepad: Rapid C 
        public string md__input__port4__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 4, 6-button Gamepad: Rapid X  
        public string md__input__port4__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 4, 6-button Gamepad: Rapid Y  
        public string md__input__port4__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 4, 6-button Gamepad: Rapid Z 
        public string md__input__port4__gamepad6__right { get; set; }                            // md, Virtual Port 4, 6-button Gamepad: RIGHT →
        public string md__input__port4__gamepad6__start { get; set; }                            // md, Virtual Port 4, 6-button Gamepad: Start
        public string md__input__port4__gamepad6__up { get; set; }                               // md, Virtual Port 4, 6-button Gamepad: UP ↑
        public string md__input__port4__gamepad6__x { get; set; }                                // md, Virtual Port 4, 6-button Gamepad: X        
        public string md__input__port4__gamepad6__y { get; set; }                                // md, Virtual Port 4, 6-button Gamepad: Y   
        public string md__input__port4__gamepad6__z { get; set; }                                // md, Virtual Port 4, 6-button Gamepad: Z  

        public string md__input__port5__gamepad6__a { get; set; }                                // md, Virtual Port 5, 6-button Gamepad: A         
        public string md__input__port5__gamepad6__b { get; set; }                                // md, Virtual Port 5, 6-button Gamepad: B   
        public string md__input__port5__gamepad6__c { get; set; }                                // md, Virtual Port 5, 6-button Gamepad: C   
        public string md__input__port5__gamepad6__down { get; set; }                             // md, Virtual Port 5, 6-button Gamepad: DOWN ↓
        public string md__input__port5__gamepad6__left { get; set; }                             // md, Virtual Port 5, 6-button Gamepad: LEFT ←
        public string md__input__port5__gamepad6__mode { get; set; }                             // md, Virtual Port 5, 6-Button Gamepad: Mode
        public string md__input__port5__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 5, 6-button Gamepad: Rapid A  
        public string md__input__port5__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 5, 6-button Gamepad: Rapid B  
        public string md__input__port5__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 5, 6-button Gamepad: Rapid C 
        public string md__input__port5__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 5, 6-button Gamepad: Rapid X  
        public string md__input__port5__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 5, 6-button Gamepad: Rapid Y  
        public string md__input__port5__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 5, 6-button Gamepad: Rapid Z 
        public string md__input__port5__gamepad6__right { get; set; }                            // md, Virtual Port 5, 6-button Gamepad: RIGHT →
        public string md__input__port5__gamepad6__start { get; set; }                            // md, Virtual Port 5, 6-button Gamepad: Start
        public string md__input__port5__gamepad6__up { get; set; }                               // md, Virtual Port 5, 6-button Gamepad: UP ↑
        public string md__input__port5__gamepad6__x { get; set; }                                // md, Virtual Port 5, 6-button Gamepad: X        
        public string md__input__port5__gamepad6__y { get; set; }                                // md, Virtual Port 5, 6-button Gamepad: Y   
        public string md__input__port5__gamepad6__z { get; set; }                                // md, Virtual Port 5, 6-button Gamepad: Z  

        public string md__input__port6__gamepad6__a { get; set; }                                // md, Virtual Port 6, 6-button Gamepad: A         
        public string md__input__port6__gamepad6__b { get; set; }                                // md, Virtual Port 6, 6-button Gamepad: B   
        public string md__input__port6__gamepad6__c { get; set; }                                // md, Virtual Port 6, 6-button Gamepad: C   
        public string md__input__port6__gamepad6__down { get; set; }                             // md, Virtual Port 6, 6-button Gamepad: DOWN ↓
        public string md__input__port6__gamepad6__left { get; set; }                             // md, Virtual Port 6, 6-button Gamepad: LEFT ←
        public string md__input__port6__gamepad6__mode { get; set; }                             // md, Virtual Port 6, 6-Button Gamepad: Mode
        public string md__input__port6__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 6, 6-button Gamepad: Rapid A  
        public string md__input__port6__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 6, 6-button Gamepad: Rapid B  
        public string md__input__port6__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 6, 6-button Gamepad: Rapid C 
        public string md__input__port6__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 6, 6-button Gamepad: Rapid X  
        public string md__input__port6__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 6, 6-button Gamepad: Rapid Y  
        public string md__input__port6__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 6, 6-button Gamepad: Rapid Z 
        public string md__input__port6__gamepad6__right { get; set; }                            // md, Virtual Port 6, 6-button Gamepad: RIGHT →
        public string md__input__port6__gamepad6__start { get; set; }                            // md, Virtual Port 6, 6-button Gamepad: Start
        public string md__input__port6__gamepad6__up { get; set; }                               // md, Virtual Port 6, 6-button Gamepad: UP ↑
        public string md__input__port6__gamepad6__x { get; set; }                                // md, Virtual Port 6, 6-button Gamepad: X        
        public string md__input__port6__gamepad6__y { get; set; }                                // md, Virtual Port 6, 6-button Gamepad: Y   
        public string md__input__port6__gamepad6__z { get; set; }                                // md, Virtual Port 6, 6-button Gamepad: Z  

        public string md__input__port7__gamepad6__a { get; set; }                                // md, Virtual Port 7, 6-button Gamepad: A         
        public string md__input__port7__gamepad6__b { get; set; }                                // md, Virtual Port 7, 6-button Gamepad: B   
        public string md__input__port7__gamepad6__c { get; set; }                                // md, Virtual Port 7, 6-button Gamepad: C   
        public string md__input__port7__gamepad6__down { get; set; }                             // md, Virtual Port 7, 6-button Gamepad: DOWN ↓
        public string md__input__port7__gamepad6__left { get; set; }                             // md, Virtual Port 7, 6-button Gamepad: LEFT ←
        public string md__input__port7__gamepad6__mode { get; set; }                             // md, Virtual Port 7, 6-Button Gamepad: Mode
        public string md__input__port7__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 7, 6-button Gamepad: Rapid A  
        public string md__input__port7__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 7, 6-button Gamepad: Rapid B  
        public string md__input__port7__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 7, 6-button Gamepad: Rapid C 
        public string md__input__port7__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 7, 6-button Gamepad: Rapid X  
        public string md__input__port7__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 7, 6-button Gamepad: Rapid Y  
        public string md__input__port7__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 7, 6-button Gamepad: Rapid Z 
        public string md__input__port7__gamepad6__right { get; set; }                            // md, Virtual Port 7, 6-button Gamepad: RIGHT →
        public string md__input__port7__gamepad6__start { get; set; }                            // md, Virtual Port 7, 6-button Gamepad: Start
        public string md__input__port7__gamepad6__up { get; set; }                               // md, Virtual Port 7, 6-button Gamepad: UP ↑
        public string md__input__port7__gamepad6__x { get; set; }                                // md, Virtual Port 7, 6-button Gamepad: X        
        public string md__input__port7__gamepad6__y { get; set; }                                // md, Virtual Port 7, 6-button Gamepad: Y   
        public string md__input__port7__gamepad6__z { get; set; }                                // md, Virtual Port 7, 6-button Gamepad: Z  

        public string md__input__port8__gamepad6__a { get; set; }                                // md, Virtual Port 8, 6-button Gamepad: A         
        public string md__input__port8__gamepad6__b { get; set; }                                // md, Virtual Port 8, 6-button Gamepad: B   
        public string md__input__port8__gamepad6__c { get; set; }                                // md, Virtual Port 8, 6-button Gamepad: C   
        public string md__input__port8__gamepad6__down { get; set; }                             // md, Virtual Port 8, 6-button Gamepad: DOWN ↓
        public string md__input__port8__gamepad6__left { get; set; }                             // md, Virtual Port 8, 6-button Gamepad: LEFT ←
        public string md__input__port8__gamepad6__mode { get; set; }                             // md, Virtual Port 8, 6-Button Gamepad: Mode
        public string md__input__port8__gamepad6__rapid_a { get; set; }                          // md, Virtual Port 8, 6-button Gamepad: Rapid A  
        public string md__input__port8__gamepad6__rapid_b { get; set; }                          // md, Virtual Port 8, 6-button Gamepad: Rapid B  
        public string md__input__port8__gamepad6__rapid_c { get; set; }                          // md, Virtual Port 8, 6-button Gamepad: Rapid C 
        public string md__input__port8__gamepad6__rapid_x { get; set; }                          // md, Virtual Port 8, 6-button Gamepad: Rapid X  
        public string md__input__port8__gamepad6__rapid_y { get; set; }                          // md, Virtual Port 8, 6-button Gamepad: Rapid Y  
        public string md__input__port8__gamepad6__rapid_z { get; set; }                          // md, Virtual Port 8, 6-button Gamepad: Rapid Z 
        public string md__input__port8__gamepad6__right { get; set; }                            // md, Virtual Port 8, 6-button Gamepad: RIGHT →
        public string md__input__port8__gamepad6__start { get; set; }                            // md, Virtual Port 8, 6-button Gamepad: Start
        public string md__input__port8__gamepad6__up { get; set; }                               // md, Virtual Port 8, 6-button Gamepad: UP ↑
        public string md__input__port8__gamepad6__x { get; set; }                                // md, Virtual Port 8, 6-button Gamepad: X        
        public string md__input__port8__gamepad6__y { get; set; }                                // md, Virtual Port 8, 6-button Gamepad: Y   
        public string md__input__port8__gamepad6__z { get; set; }                                // md, Virtual Port 8, 6-button Gamepad: Z  

        /* Mega Mouse */
        public string md__input__port1__megamouse__left { get; set; }                           // md, Virtual Port 1, Sega Mega Mouse: Left Button
        public string md__input__port1__megamouse__middle { get; set; }                         // md, Virtual Port 1, Sega Mega Mouse: Middle Button
        public string md__input__port1__megamouse__right { get; set; }                          // md, Virtual Port 1, Sega Mega Mouse: Right Button
        public string md__input__port1__megamouse__start { get; set; }                          // md, Virtual Port 1, Sega Mega Mouse: Start Button

        public string md__input__port2__megamouse__left { get; set; }                           // md, Virtual Port 2, Sega Mega Mouse: Left Button
        public string md__input__port2__megamouse__middle { get; set; }                         // md, Virtual Port 2, Sega Mega Mouse: Middle Button
        public string md__input__port2__megamouse__right { get; set; }                          // md, Virtual Port 2, Sega Mega Mouse: Right Button
        public string md__input__port2__megamouse__start { get; set; }                          // md, Virtual Port 2, Sega Mega Mouse: Start Button

        public string md__input__port3__megamouse__left { get; set; }                           // md, Virtual Port 3, Sega Mega Mouse: Left Button
        public string md__input__port3__megamouse__middle { get; set; }                         // md, Virtual Port 3, Sega Mega Mouse: Middle Button
        public string md__input__port3__megamouse__right { get; set; }                          // md, Virtual Port 3, Sega Mega Mouse: Right Button
        public string md__input__port3__megamouse__start { get; set; }                          // md, Virtual Port 3, Sega Mega Mouse: Start Button

        public string md__input__port4__megamouse__left { get; set; }                           // md, Virtual Port 4, Sega Mega Mouse: Left Button
        public string md__input__port4__megamouse__middle { get; set; }                         // md, Virtual Port 4, Sega Mega Mouse: Middle Button
        public string md__input__port4__megamouse__right { get; set; }                          // md, Virtual Port 4, Sega Mega Mouse: Right Button
        public string md__input__port4__megamouse__start { get; set; }                          // md, Virtual Port 4, Sega Mega Mouse: Start Button

        public string md__input__port5__megamouse__left { get; set; }                           // md, Virtual Port 5, Sega Mega Mouse: Left Button
        public string md__input__port5__megamouse__middle { get; set; }                         // md, Virtual Port 5, Sega Mega Mouse: Middle Button
        public string md__input__port5__megamouse__right { get; set; }                          // md, Virtual Port 5, Sega Mega Mouse: Right Button
        public string md__input__port5__megamouse__start { get; set; }                          // md, Virtual Port 5, Sega Mega Mouse: Start Button

        public string md__input__port6__megamouse__left { get; set; }                           // md, Virtual Port 6, Sega Mega Mouse: Left Button
        public string md__input__port6__megamouse__middle { get; set; }                         // md, Virtual Port 6, Sega Mega Mouse: Middle Button
        public string md__input__port6__megamouse__right { get; set; }                          // md, Virtual Port 6, Sega Mega Mouse: Right Button
        public string md__input__port6__megamouse__start { get; set; }                          // md, Virtual Port 6, Sega Mega Mouse: Start Button

        public string md__input__port7__megamouse__left { get; set; }                           // md, Virtual Port 7, Sega Mega Mouse: Left Button
        public string md__input__port7__megamouse__middle { get; set; }                         // md, Virtual Port 7, Sega Mega Mouse: Middle Button
        public string md__input__port7__megamouse__right { get; set; }                          // md, Virtual Port 7, Sega Mega Mouse: Right Button
        public string md__input__port7__megamouse__start { get; set; }                          // md, Virtual Port 7, Sega Mega Mouse: Start Button

        public string md__input__port8__megamouse__left { get; set; }                           // md, Virtual Port 8, Sega Mega Mouse: Left Button
        public string md__input__port8__megamouse__middle { get; set; }                         // md, Virtual Port 8, Sega Mega Mouse: Middle Button
        public string md__input__port8__megamouse__right { get; set; }                          // md, Virtual Port 8, Sega Mega Mouse: Right Button
        public string md__input__port8__megamouse__start { get; set; }                          // md, Virtual Port 8, Sega Mega Mouse: Start Button

        // nes

        /* Famicon Expansion Port */
        /* Arkanoid Paddle */
        public string nes__input__fcexp__arkanoid__button { get; set; }                         // nes, Famicom Expansion Port, Arkanoid Paddle: Button
        public string nes__input__fcexp__arkanoid__x_axis { get; set; }                         // nes, Famicom Expansion Port, Arkanoid Paddle: X Axis

        /* Family Keyboard (skip)*/

        /* Oeka Kids Tablet */
        public string nes__input__fcexp__oekakids__button { get; set; }                         // nes, Famicom Expansion Port, Oeka Kids Tablet: Button
        public string nes__input__fcexp__oekakids__x_axis { get; set; }                         // nes, Famicom Expansion Port, Oeka Kids Tablet: X Axis
        public string nes__input__fcexp__oekakids__y_axis { get; set; }                         // Famicom Expansion Port, Oeka Kids Tablet: Y Axis

        /* Party Tap */
        public string nes__input__fcexp__partytap__buzzer_1 { get; set; }                       // nes, Famicom Expansion Port, Party Tap: Buzzer 1
        public string nes__input__fcexp__partytap__buzzer_2 { get; set; }                       // nes, Famicom Expansion Port, Party Tap: Buzzer 2
        public string nes__input__fcexp__partytap__buzzer_3 { get; set; }                       // nes, Famicom Expansion Port, Party Tap: Buzzer 3
        public string nes__input__fcexp__partytap__buzzer_4 { get; set; }                       // nes, Famicom Expansion Port, Party Tap: Buzzer 4
        public string nes__input__fcexp__partytap__buzzer_5 { get; set; }                       // nes, Famicom Expansion Port, Party Tap: Buzzer 5
        public string nes__input__fcexp__partytap__buzzer_6 { get; set; }                       // nes, Famicom Expansion Port, Party Tap: Buzzer 6

        /* Space Shadow Gun */
        public string nes__input__fcexp__shadow__away_trigger { get; set; }                     // nes, Famicom Expansion Port, Space Shadow Gun: Away Trigger
        public string nes__input__fcexp__shadow__trigger { get; set; }                          // nes, Famicom Expansion Port, Space Shadow Gun: Trigger
        public string nes__input__fcexp__shadow__x_axis { get; set; }                           // nes, Famicom Expansion Port, Space Shadow Gun: X Axis
        public string nes__input__fcexp__shadow__y_axis { get; set; }                           // nes, Famicom Expansion Port, Space Shadow Gun: Y Axis

        /* Normal Inputs */
        
        /* Gamepads */
        public string nes__input__port1__gamepad__a { get; set; }                               // nes, Port 1, Gamepad: A
        public string nes__input__port1__gamepad__b { get; set; }                               // nes, Port 1, Gamepad: B
        public string nes__input__port1__gamepad__down { get; set; }                            // nes, Port 1, Gamepad: DOWN ↓
        public string nes__input__port1__gamepad__left { get; set; }                            // nes, Port 1, Gamepad: LEFT ←
        public string nes__input__port1__gamepad__rapid_a { get; set; }                         // nes, Port 1, Gamepad: Rapid A
        public string nes__input__port1__gamepad__rapid_b { get; set; }                         // nes, Port 1, Gamepad: Rapid B
        public string nes__input__port1__gamepad__right { get; set; }                           // nes, Port 1, Gamepad: RIGHT →
        public string nes__input__port1__gamepad__select { get; set; }                          // nes, Port 1, Gamepad: SELECT
        public string nes__input__port1__gamepad__start { get; set; }                           // nes, Port 1, Gamepad: START
        public string nes__input__port1__gamepad__up { get; set; }                              // nes, Port 1, Gamepad: UP ↑

        public string nes__input__port2__gamepad__a { get; set; }                               // nes, Port 2, Gamepad: A
        public string nes__input__port2__gamepad__b { get; set; }                               // nes, Port 2, Gamepad: B
        public string nes__input__port2__gamepad__down { get; set; }                            // nes, Port 2, Gamepad: DOWN ↓
        public string nes__input__port2__gamepad__left { get; set; }                            // nes, Port 2, Gamepad: LEFT ←
        public string nes__input__port2__gamepad__rapid_a { get; set; }                         // nes, Port 2, Gamepad: Rapid A
        public string nes__input__port2__gamepad__rapid_b { get; set; }                         // nes, Port 2, Gamepad: Rapid B
        public string nes__input__port2__gamepad__right { get; set; }                           // nes, Port 2, Gamepad: RIGHT →
        public string nes__input__port2__gamepad__select { get; set; }                          // nes, Port 2, Gamepad: SELECT
        public string nes__input__port2__gamepad__start { get; set; }                           // nes, Port 2, Gamepad: START
        public string nes__input__port2__gamepad__up { get; set; }                              // nes, Port 2, Gamepad: UP ↑

        public string nes__input__port3__gamepad__a { get; set; }                               // nes, Port 3, Gamepad: A
        public string nes__input__port3__gamepad__b { get; set; }                               // nes, Port 3, Gamepad: B
        public string nes__input__port3__gamepad__down { get; set; }                            // nes, Port 3, Gamepad: DOWN ↓
        public string nes__input__port3__gamepad__left { get; set; }                            // nes, Port 3, Gamepad: LEFT ←
        public string nes__input__port3__gamepad__rapid_a { get; set; }                         // nes, Port 3, Gamepad: Rapid A
        public string nes__input__port3__gamepad__rapid_b { get; set; }                         // nes, Port 3, Gamepad: Rapid B
        public string nes__input__port3__gamepad__right { get; set; }                           // nes, Port 3, Gamepad: RIGHT →
        public string nes__input__port3__gamepad__select { get; set; }                          // nes, Port 3, Gamepad: SELECT
        public string nes__input__port3__gamepad__start { get; set; }                           // nes, Port 3, Gamepad: START
        public string nes__input__port3__gamepad__up { get; set; }                              // nes, Port 3, Gamepad: UP ↑

        public string nes__input__port4__gamepad__a { get; set; }                               // nes, Port 4, Gamepad: A
        public string nes__input__port4__gamepad__b { get; set; }                               // nes, Port 4, Gamepad: B
        public string nes__input__port4__gamepad__down { get; set; }                            // nes, Port 4, Gamepad: DOWN ↓
        public string nes__input__port4__gamepad__left { get; set; }                            // nes, Port 4, Gamepad: LEFT ←
        public string nes__input__port4__gamepad__rapid_a { get; set; }                         // nes, Port 4, Gamepad: Rapid A
        public string nes__input__port4__gamepad__rapid_b { get; set; }                         // nes, Port 4, Gamepad: Rapid B
        public string nes__input__port4__gamepad__right { get; set; }                           // nes, Port 4, Gamepad: RIGHT →
        public string nes__input__port4__gamepad__select { get; set; }                          // nes, Port 4, Gamepad: SELECT
        public string nes__input__port4__gamepad__start { get; set; }                           // nes, Port 4, Gamepad: START
        public string nes__input__port4__gamepad__up { get; set; }                              // nes, Port 4, Gamepad: UP ↑

        /* skip arkanoid paddle */

        /* skip powerpaddle */

        /* skip zapper */

        // ngp
        public string ngp__input__builtin__gamepad__a { get; set; }                             // ngp, Built-In, Gamepad: A
        public string ngp__input__builtin__gamepad__b { get; set; }                             // ngp, Built-In, Gamepad: B
        public string ngp__input__builtin__gamepad__down { get; set; }                          // ngp, Built-In, Gamepad: DOWN ↓
        public string ngp__input__builtin__gamepad__left { get; set; }                          // ngp, Built-In, Gamepad: LEFT ←
        public string ngp__input__builtin__gamepad__option { get; set; }                        // ngp, Built-In, Gamepad: OPTION
        public string ngp__input__builtin__gamepad__rapid_a { get; set; }                       // ngp, Built-In, Gamepad: Rapid A
        public string ngp__input__builtin__gamepad__rapid_b { get; set; }                       // ngp, Built-In, Gamepad: Rapid B
        public string ngp__input__builtin__gamepad__right { get; set; }                         // ngp, Built-In, Gamepad: RIGHT →
        public string ngp__input__builtin__gamepad__up { get; set; }                            // ngp, Built-In, Gamepad: UP ↑

        public MednafenControls()
        {
            // load config settings from disk
        }
    }
}
