import { LastPlayedPipe } from './last-played.pipe';

describe('LastPlayedPipe', () => {
  it('create an instance', () => {
    const pipe = new LastPlayedPipe();
    expect(pipe).toBeTruthy();
  });
});
