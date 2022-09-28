import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SideBarColorService {
  storedTheme: string = '';
  storedThemeBody: string = localStorage.getItem('theme-color-body');
  
  logoName = "";
  constructor() { }
  setTheme(theme){
   
    localStorage.setItem('theme-color', theme);
    this.storedTheme = localStorage.getItem('theme-color');
    if(theme == "theme-purple"){
      localStorage.setItem('logoName', "ERP-Final-logo.png");
      this.logoName = "ERP-Final-logo.png"
    }
    else if(theme == "theme-blue"){
      localStorage.setItem('logoName', "ERP-Final-logo.png");
      this.logoName = "ERP-Final-logo.png"
    }
    else if(theme == "theme-default"){
      localStorage.setItem('logoName', "ERP-Final-logo.png");
      this.logoName = "ERP-Final-logo.png"
    }
    var bodyTheme = theme + "-body"
    localStorage.setItem('theme-color-body', bodyTheme);
    this.storedThemeBody = localStorage.getItem('theme-color-body')
  }
  getLogo(){
    if(localStorage.getItem('theme-color') == null){
      localStorage.setItem('theme-color', "theme-default");
      this.storedTheme= localStorage.getItem('theme-color');
      localStorage.setItem('logoName', "ERP-Final-logo.png");
      this.logoName = localStorage.getItem('logoName')
      //this.logoName = "ERP-Final-logo.png"
    }else{
      this.storedTheme= localStorage.getItem('theme-color');
      this.logoName= localStorage.getItem('logoName');
    }
  }
}
