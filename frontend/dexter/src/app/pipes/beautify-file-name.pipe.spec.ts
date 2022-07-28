import { BeautifyFileNamePipe } from './beautify-file-name.pipe';

describe('BeautifyFileNamePipe', () => {
  it('create an instance', () => {
    const pipe = new BeautifyFileNamePipe();
    expect(pipe).toBeTruthy();
  });
});
