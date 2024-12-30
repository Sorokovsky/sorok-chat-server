import { AlreadyExistsException } from "@exceptions/base/already-exists.exception";

export class PathAlreadyExistsException extends AlreadyExistsException {
  constructor(path: string) {
    super("File or directory", "path", path);
  }
}
