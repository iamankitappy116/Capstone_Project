import { AutoFocus } from './auto-focus';
import { ElementRef } from '@angular/core';

describe('AutoFocus', () => {
  it('should create an instance', () => {
    const directive = new AutoFocus(new ElementRef(null));
    expect(directive).toBeTruthy();
  });
});
