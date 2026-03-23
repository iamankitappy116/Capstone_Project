import { Directive, ElementRef, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appAutoFocus]',
  standalone: false,
})
export class AutoFocus implements AfterViewInit {
  constructor(private el: ElementRef) {}

  ngAfterViewInit(): void {
    this.el.nativeElement.focus();
  }
}
